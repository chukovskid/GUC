using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {

        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> clodinaryConfig)
        {
            _cloudinaryConfig = clodinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account( 
            _cloudinaryConfig.Value.CloudName,
            _cloudinaryConfig.Value.ApiKey,
            _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);


        }


        [HttpPost("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {

            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }




        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm] PhotoForCreationDto photoForCreationDto)
        { // 107
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var userFromRepo = await _repo.GetUser(userId);


            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult(); // so ova go zacuvuvame Resultot od Cloudinady

            if (file.Length > 0)
            { // ako postoi file pocnuvame da go dodavame u cloudinary {

                using (var stream = file.OpenReadStream())
                { // is gonna read our uploaded file in memory
                    var uploadParams = new ImageUploadParams()
                    { // mu gi davame na clodinary nasite uploadParams
                        File = new FileDescription(file.Name, stream), // ? // File = Fajlot ili ULR sejvnuva u nego (otvoru mu desc)
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face") // Croping pred da ja Sejvnam

                    };
                    uploadResult = _cloudinary.Upload(uploadParams); // se sto e sejvnato ke bide u uploadResult pa zatoa mu gi doodavam Paarametrite(iploadParams)
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto); 

            if (!userFromRepo.Photos.Any(u => u.IsMain))
            { 
                photo.IsMain = true; // prvava slika ke bide Profilna
            }

            userFromRepo.Photos.Add(photo); 

            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id }, photoToReturn); 
            }

            return BadRequest("Ne moze da se dodade slikata!");

        }




        // api/users/{userId}/photos  /id/setMain
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        { 
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized(); 
            var user = await _repo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized(); 

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already your main photo bro");

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Main slikata ne uspea da se smeni");

        }

        //Delete//: api/users/{userId}/photos  /id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        { // 113

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized(); 
            var user = await _repo.GetUser(userId);
            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized(); 
            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("You Cant delete your main Photo!");

            if (photoFromRepo.PhotoId != null)
            { // ako voopsto postoi slikava, brisi i od Cloudinary

                var deleteParams = new DeletionParams(photoFromRepo.PhotoId); 
                var result = _cloudinary.Destroy(deleteParams); 

                if (result.Result == "ok")
                {
                    _repo.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PhotoId == null)
            { // a ako Ne postoi brisi od Repo
                _repo.Delete(photoFromRepo);
            }

            
            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Slikata ne uspea da se izbrise");



        }
    }
}
