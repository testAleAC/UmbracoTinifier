﻿using System;
using System.Collections.Generic;
using System.Linq;
using Tinifier.Core.Repository.Interfaces;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Tinifier.Core.Repository.Realization
{
    public class TImageRepository : IEntityReader<Media>, IImageRepository<Media>
    {
        private IContentTypeService _contentTypeService;
        private IMediaService _mediaService;
        private UmbracoDatabase _database;

        public TImageRepository()
        {
            _mediaService = ApplicationContext.Current.Services.MediaService;
            _database = ApplicationContext.Current.DatabaseContext.Database;
            _contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
        }

        public IEnumerable<Media> GetAll()
        {
            var mediaList = new List<Media>();
            var mediaItems = _mediaService.GetMediaOfMediaType(_contentTypeService.GetMediaType("image").Id);

            foreach(var item in mediaItems)
            {
                mediaList.Add(item as Media);
            }

            return mediaList;
        }

        public Media GetByKey(int Id)
        {
            var mediaItem = _mediaService.GetById(Id) as Media;

            return mediaItem;
        }

        public void UpdateItem(IMediaService mediaService, Media mediaItem)
        {
            mediaItem.UpdateDate = DateTime.UtcNow;
            mediaService.Save(mediaItem);
        }

        public IEnumerable<Media> GetOptimizedItems()
        {
            var mediaList = new List<Media>();
            var query = new Sql("SELECT ImageId FROM TinifierResponseHistory WHERE IsOptimized = 'true'");
            var historyIds = _database.Fetch<int>(query);

            var mediaItems = _mediaService.
                             GetMediaOfMediaType(_contentTypeService.GetMediaType("image").Id).
                             Where(item => historyIds.Contains(item.Id));

            foreach (var item in mediaItems)
            {
                mediaList.Add(item as Media);
            }

            return mediaList;
        }
    }
}