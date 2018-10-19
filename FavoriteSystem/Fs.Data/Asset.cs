namespace Fs.Data
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// A slimmed down version of the asset.
    /// </summary>
    public class SlimAsset
    {

        /// <summary>
        /// The id of the asset
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the asset
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The number of times the asset has been upvoted
        /// </summary>
        [JsonProperty("favoriteCount")]
        public int FavoriteCount { get; set; }

        /// <summary>
        /// Dummy data stuff
        /// </summary>
        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }
    }

    /// <summary>
    /// A class to represent a full asset.
    /// </summary>
    public class Asset : SlimAsset
    {
        /// <summary>
        /// Dummy data stuff
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Dummy data stuff
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// Dummy data stuff
        /// </summary>
        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
