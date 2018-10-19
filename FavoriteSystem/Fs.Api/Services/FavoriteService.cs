namespace Fs.Api.Services
{
    using Fs.Data;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// Service to handle favorite toggle requests.
    /// </summary>
    public class FavoriteService
    {

        private DocumentClient docClient;

        private Uri peopleCollectionUri = UriFactory.CreateDocumentCollectionUri("fs", "people");
        private Uri assetsCollectionUri = UriFactory.CreateDocumentCollectionUri("fs", "assets");

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FavoriteService()
        {
            this.docClient = new DocumentClient(new Uri(ConfigurationManager.AppSettings["docDbEndpoint"]), ConfigurationManager.AppSettings["docDbKey"]);
        }


        public async Task<List<AssetFavorite>> ListPersonFavoritesAsync(string personId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Favorite an asset.
        /// </summary>
        /// <param name="assetId">The asset to favorite</param>
        /// <param name="personId">The person that is executing the command</param>
        /// <returns></returns>
        public async Task AddToFavoritesAsync(string assetId, string personId)
        {
            try
            {
                var asset = await this.FetchAssetByIdAsync(assetId); // Cache aside this?!
                var slim = new SlimAsset(asset);
                var favorite = new AssetFavorite
                {
                    Id = AssetFavorite.CreateId(slim.Id, personId),
                    Asset = slim,
                    PersonId = personId
                };

                await this.docClient.CreateDocumentAsync(peopleCollectionUri, favorite);
            }
            catch (DocumentClientException dce)
            {
                ; //debug this
            }
            catch (Exception ex)
            {

                ; //debug this.
            }
        }

        /// <summary>
        /// Un-Favorite an asset.
        /// </summary>
        /// <param name="assetId">The asset to un-favorite</param>
        /// <param name="personId">The person that is executing the command</param>
        /// <returns></returns>
        public async Task RemoveFromFavoritesAsync(string assetId, string personId)
        {
            try
            {
                var link = UriFactory.CreateDocumentUri("fs", "people", AssetFavorite.CreateId(assetId, personId));
                await this.docClient.DeleteDocumentAsync(link);
            }
            catch (DocumentClientException dce)
            {
                if (dce.StatusCode == HttpStatusCode.NotFound)
                {
                    //cool, already gone?
                    return;
                }
            }
            catch (Exception ex)
            {

                ; //debug this
            }
        }


        /// <summary>
        /// Queues an update command to update an asset's favorite count.
        /// </summary>
        /// <param name="assetId">The asset to update</param>
        /// <param name="delta">The amount to increase/descrease by</param>
        /// <returns></returns>
        public async Task QueueUpdateFavoriteCountAsync(string assetId, int delta)
        {
            try
            {
                var asset = await this.FetchAssetByIdAsync(assetId);
                asset.FavoriteCount += delta;
                await this.docClient.UpsertDocumentAsync( //should I upsert... ? what about a race condition here?
                    assetsCollectionUri, 
                    asset, 
                    new RequestOptions { ConsistencyLevel = ConsistencyLevel.Eventual });
            }
            catch (Exception)
            {
                ; //debug this
                throw;
            }
        }

        private async Task<Asset> FetchAssetByIdAsync(string assetId)
        {
            var feedOpts = new FeedOptions { MaxItemCount = 1 };
            var query = this.docClient.CreateDocumentQuery<Asset>(this.assetsCollectionUri, $"select * from c where c.id = '{assetId}'", feedOpts).AsDocumentQuery();
            var result = await query.ExecuteNextAsync<Asset>();

            return result.FirstOrDefault();
        }

        private async Task<Person> FetchPersonByIdAsync(string personId)
        {
            var collection = UriFactory.CreateDocumentCollectionUri("fs", "people");
            var feedOpts = new FeedOptions { MaxItemCount = 1 };
            var query = this.docClient.CreateDocumentQuery<Person>(
                this.peopleCollectionUri,
                $"select * from c where c.personId = '{personId}' c.id = '{Person.MakeProfileDocId(personId)}'",
                feedOpts)
                .AsDocumentQuery();

            var result = await query.ExecuteNextAsync<Person>();

            return result.FirstOrDefault();
        }

    }
}