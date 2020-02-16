using System;
using System.Collections.Generic;
using System.Linq;
using ApiBackEndShared.Enumerations;

namespace ApiBackEndShared.ViewModels
{
    public class ProfileResponseViewModel
    {
        #region Properties

        public List<Guid> Ids { get; set; }

        public List<Qualities> Qualities { get; set; }

        public Guid NonNullableId { get; set; }

        public Guid? NullableId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public decimal Amount { get; set; }

        public Qualities? NullableQuality { get; set; }

        public Qualities NonNullableQuality { get; set; }

        public HttpFileBaseResponseViewModel Photo { get; set; }

        public List<HttpFileBaseResponseViewModel> Photos { get; set; }

        #endregion

        #region Constructor

        public ProfileResponseViewModel(ProfileViewModel model)
        {
            if (model == null)
                return;

            Ids = model.Ids;
            Qualities = model.Qualities;
            NonNullableId = model.NonNullableId;
            NullableId = model.NullableId;
            Name = model.Name;
            Age = model.Age;
            Amount = model.Amount;
            NullableQuality = model.NullableQuality;
            NonNullableQuality = model.NonNullableQuality;
            Photo = model.Photo != null ? new HttpFileBaseResponseViewModel(model.Photo) : null;

            if (model.Photos != null && model.Photos.Count > 0)
                Photos = model.Photos.Select(photo => new HttpFileBaseResponseViewModel(photo)).ToList();

        }

        #endregion

        #region Methods



        #endregion
    }
}