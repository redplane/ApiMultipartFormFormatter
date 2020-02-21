using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ApiBackEnd.IntegrationTest.Shared.Extensions;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.IntegrationTest.Shared.ViewModels
{
    public class RawProfileViewModel
    {
        public List<string> Ids { get; set; }

        public List<string> Qualities { get; set; }

        public string NonNullableId { get; set; }

        public string NullableId { get; set; }

        public string Name { get; set; }

        public string Age { get; set; }

        public string Amount { get; set; }

        public string NullableQuality { get; set; }

        public string NonNullableQuality { get; set; }

        public HttpFileBase Photo { get; set; }

        public List<HttpFileBase> Photos { get; set; }

        #region Methods

        public void ExtendHttpContent(MultipartFormDataContent httpContent, string prefix = default)
        {
            if (Ids != null && Ids.Count > 0)
                for (var id = 0; id < Ids.Count; id++)
                    httpContent.Add(new StringContent(Ids[id], Encoding.UTF8),
                        $"{prefix}{nameof(Ids)}[{id}]");

            if (Qualities != null && Qualities.Count > 0)
                for (var id = 0; id < Qualities.Count; id++)
                    httpContent.Add(new StringContent(Qualities[id], Encoding.UTF8),
                        $"{prefix}{nameof(Qualities)}[{id}]");

            if (NonNullableId != null)
                httpContent.Add(new StringContent(NonNullableId, Encoding.UTF8),
                    $"{prefix}{nameof(NonNullableId)}");

            if (NullableId != null)
                httpContent.Add(new StringContent(NullableId, Encoding.UTF8),
                    $"{prefix}{nameof(NullableId)}");

            if (Name != null)
                httpContent.Add(new StringContent(Name, Encoding.UTF8),
                    $"{prefix}{nameof(Name)}");

            if (Age != null)
                httpContent.Add(new StringContent(Age, Encoding.UTF8),
                    $"{prefix}{nameof(Age)}");

            if (Amount != null)
                httpContent.Add(new StringContent(Amount, Encoding.UTF8),
                    $"{prefix}{nameof(Amount)}");

            if (NullableQuality != null)
                httpContent.Add(new StringContent(NullableQuality, Encoding.UTF8),
                    $"{prefix}{nameof(NullableQuality)}");

            if (NonNullableQuality != null)
                httpContent.Add(new StringContent(NonNullableQuality, Encoding.UTF8),
                    $"{prefix}{nameof(NonNullableQuality)}");

            if (Photo != null)
            {
#if NETFRAMEWORK
                httpContent.Add(Photo.ToByteArrayContent($"{prefix}{nameof(Photo)}"));
#elif NETSTANDARD
                httpContent.Add(new ByteArrayContent(Photo.ToBytes()), $"{prefix}{nameof(Photo)}", Photo.FileName);
#endif
            }

            if (Photos != null && Photos.Count > 0)
            {
                for (var photoId = 0; photoId < Photos.Count; photoId++)
                {
#if NETFRAMEWORK
                    httpContent.Add(Photos[photoId].ToByteArrayContent($"{prefix}{nameof(Photos)}[{photoId}]"));
#elif NETSTANDARD
                    httpContent.Add(new ByteArrayContent(Photos[photoId].ToBytes()),$"{prefix}{nameof(Photos)}[{photoId}]", Photos[photoId].FileName);
#endif
                }
            }
                
        }

#endregion
    }
}