namespace Fs.Data
{
    using Newtonsoft.Json;

    /// <summary>
    /// Class to represent a person in the system.
    /// PartitionKey: /personId
    /// DocType: 'profile'
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Similar to the partition key, the id is the document id of the profile. Represented as 'profile-{personId}'
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// PARTITION KEY
        /// The person id, unique to each person.
        /// </summary>
        [JsonProperty("personId")]
        public string PersonId { get; set; }

        /// <summary>
        /// Displayed name of the person
        /// </summary>
        [JsonProperty("gamertag")]
        public string Gamertag { get; set; }

        /// <summary>
        /// Junk data to populate...
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// The document type.
        /// </summary>
        [JsonProperty("docType")]
        public string DocType => "profile";
    }

    /// <summary>
    /// Class to represent an asset a person has favorited.
    /// PartitionKey: /personId
    /// DocType: 'asset-favorite'
    /// </summary>
    public class AssetFavorite
    {
        /// <summary>
        /// Similar to the partition key, the id is the document id of the profile. Represented as 'favorite-{personId}-{assetId}'
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// PARTITION KEY
        /// The person id, unique to each person.
        /// </summary>
        [JsonProperty("personId")]
        public string PersonId { get; set; }

        /// <summary>
        /// A denormalized reference to the Asset
        /// </summary>
        [JsonProperty("asset")]
        public SlimAsset Asset { get; set; }
    }

}
