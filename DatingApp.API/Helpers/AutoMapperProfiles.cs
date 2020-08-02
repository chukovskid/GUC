using System.Linq;
using AutoMapper;
using DatingApp.API.dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<User, UserForListDto>() // delot posle ova kazuva da ide u OvojUser.Photos.IsMain.Url 
            .ForMember(dest => dest.PhotoUrl, opt => // vo UserForListDto kako promenliva PhotoUrl stavi ja ...
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url)) // ... prvata photo vo user.Photos kaj koja isMain=true
                    .ForMember(dest => dest.Age, opt => // najdi go Age vo Dto i daj mu vrednost od User.FUNKCIJA.Godini
                         opt.MapFrom(src => src.DateOfBirth.CalculateAge()));  // Ova e so da bara u MapForm(User)

            CreateMap<User, UserForDetailedDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                 .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<Message, MessageForReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));


            CreateMap<NotificationForCreationDto, Notifications>();
            CreateMap<RoomForCreationDto, Room>();
            CreateMap<Room, RoomForCreationDto>();
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>(); //  podatocite od Photo modelot im davam pat do PhotoForReturnDto
            CreateMap<PhotoForCreationDto, Photo>(); // bidejki Samo sto gi kreirav mora podatocive od PhotoForCreationDto da im dadam pravec do Photo Modelot
            CreateMap<UserForRegisterDto, User>(); // se sto ke stavam vo UserForRegisterDto ke se prati vo User
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageForReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));


        }


    }
}