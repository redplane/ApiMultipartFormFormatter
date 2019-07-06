using System.Collections.Generic;

namespace MultipartFormDataMyGet.ViewModels
{
    public class ClientResponseViewModel
    {
        #region Properties

        public List<string> Messages { get; set; }

        #endregion

        #region Constructor

        public ClientResponseViewModel(string message)
        {
            Messages = new List<string>(){message};
        }

        public ClientResponseViewModel(IList<string> messages)
        {
            Messages = new List<string>(messages);
        }

        #endregion

    }
}