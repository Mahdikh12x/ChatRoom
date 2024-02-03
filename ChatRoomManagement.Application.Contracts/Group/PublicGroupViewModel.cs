namespace ChatRoomManagement.Application.Contracts.Group
{
    public class PublicGroupViewModel
    {
        public long Id { get; set; }
        public string GroupTitle { get; set; }
        public string Token { get; set; }
        public string Picture { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
