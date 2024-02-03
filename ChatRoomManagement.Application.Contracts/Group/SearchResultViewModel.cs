namespace ChatRoomManagement.Application.Contracts.Group
{
    public class SearchResultViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Token { get; set; }
        public string Picture { get; set; }
        public bool IsUser { get; set; }
    }
}
