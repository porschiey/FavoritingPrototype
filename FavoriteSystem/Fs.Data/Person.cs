namespace Fs.Data
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Class to represent a person in the system.
    /// PartitionKey: /personId
    /// DocType: 'profile'
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Creates an id.
        /// </summary>
        /// <returns></returns>
        public static string CreateId(string personId = "")
        {
            if (string.IsNullOrWhiteSpace(personId))
                personId = Guid.NewGuid().ToString();

            return $"profile|{personId}";
        }

        /// <summary>
        /// Makes a personId string into a document id that can be queried
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public static string MakeProfileDocId(string personId)
        {
            if (string.IsNullOrWhiteSpace(personId))
                return null;

            if (personId.Contains("|"))
                return personId;

            return CreateId(personId);
        }

        /// <summary>
        /// Similar to the partition key, the id is the document id of the profile. Represented as 'profile|{personId}'
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
        /// Creates an id.
        /// </summary>
        /// <returns></returns>
        public static string CreateId(string assetId, string personId)
        {
            if (string.IsNullOrWhiteSpace(assetId))
                throw new ArgumentNullException(nameof(assetId));

            if (string.IsNullOrWhiteSpace(personId))
                throw new ArgumentNullException(nameof(personId));

            return $"asset-favorite|{assetId}|{personId}";
        }

        /// <summary>
        /// Similar to the partition key, the id is the document id of the profile. Represented as 'asset-favorite|{assetId}|{personId}'
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
