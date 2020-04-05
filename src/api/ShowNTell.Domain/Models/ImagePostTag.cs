namespace ShowNTell.Domain.Models
{
    /// <summary>
    /// A model joining an image post to a tag.
    /// </summary>
    public class ImagePostTag
    {
        public int ImagePostId { get; set; } 
        public int TagId { get; set; } 

        public ImagePost ImagePost { get; set; }
        public Tag Tag { get; set; }
    }
}