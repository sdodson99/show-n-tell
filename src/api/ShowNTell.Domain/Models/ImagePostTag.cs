namespace ShowNTell.Domain.Models
{
    public class ImagePostTag
    {
        public int ImagePostId { get; set; } 
        public int TagId { get; set; } 

        public ImagePost ImagePost { get; set; }
        public Tag Tag { get; set; }
    }
}