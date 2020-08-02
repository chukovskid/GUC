namespace DatingApp.API.Helpers
{
    public class UserParams
    { // ova ke gi ZIMA PARAMETRITE od KORISNIKOT (User) i ke ni gi vrati // 141
        public int MaxSize { get; set; } = 50; // max broj na <T> komponenti na stranata
        public int PageNumber { get; set; } = 1; // vo slucaj da nemaat izbrano -> Default=1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxSize)? MaxSize:value ;} // ako e pogolemo od 50 neka e = Max vo sprotivno = value vneseno
        }

        public int UserId { get; set; } // za da go trgnam MOMENTALNO Logiraniot User
        public string Gender { get; set; } // Da filtriram spored POL (maski/zenski)

        public int maxAge { get; set; } = 99;
        public int minAge { get; set; } = 18;
        public string  orderBy { get; set; }
        public bool Likees { get; set; } = false;
        public bool Likers { get; set; } =false;
        public string Search { get; set; }
        
    }
}