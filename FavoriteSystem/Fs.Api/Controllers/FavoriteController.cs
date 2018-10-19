namespace Fs.Api.Controllers
{
    using Fs.Api.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;

    /// <summary>
    /// Controller responsible for making favorite actions.
    /// </summary>
    public class FavoriteController : ApiController
    {

        private FavoriteService service;

        public FavoriteController()
        {
            this.service = new FavoriteService();
        }

        public string PersonId
        {
            get
            {
                if (this.Request.Headers.Contains("person"))
                    return this.Request.Headers.First(h => h.Key == "person").Value.First();

                return string.Empty;
            }
        }

        /// <summary>
        /// Marks an asset as favorited.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/favorite/add/{assetId}")]
        public async Task<HttpResponseMessage> AddFavorite(string assetId)
        {
            await this.service.AddToFavoritesAsync(assetId, this.PersonId);

            HostingEnvironment.QueueBackgroundWorkItem(async (cancelTok) =>
            {
                await this.service.QueueUpdateFavoriteCountAsync(assetId, 1);
            });

            return this.Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Marks an asset as favorited.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/favorite/remove/{assetId}")]
        public async Task<HttpResponseMessage> RemoveFavorite(string assetId)
        {
            await this.service.RemoveFromFavoritesAsync(assetId, this.PersonId);

            HostingEnvironment.QueueBackgroundWorkItem(async (cancelTok) =>
            {
                await this.service.QueueUpdateFavoriteCountAsync(assetId, -1);
            });

            return this.Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
