namespace DatingApp.API.Helpers
{
    public class MessageParams
    {
         public int MaxSize { get; set; } = 50; // max broj na <T> komponenti na stranata
        public int PageNumber { get; set; } = 1; // vo slucaj da nemaat izbrano -> Default=1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxSize)? MaxSize:value ;} // ako e pogolemo od 50 neka e = Max vo sprotivno = value vneseno
        }

        public int UserId { get; set; } // za da go trgnam MOMENTALNO Logiraniot User
        public string MessageContainer { get; set; } = "Unread";
    }
}