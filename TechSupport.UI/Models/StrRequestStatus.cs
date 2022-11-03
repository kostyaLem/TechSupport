using System.Collections.Generic;
using System.Windows.Data;
using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.UI.Converters;

namespace TechSupport.UI.ViewModels;

public sealed partial class RequestsViewModel
{
    public record StrRequestStatus
    {
        private static readonly IValueConverter _converter = new EnumToDescriptionConverter();

        public StrRequestStatus(RequestStatus requestStatus)
        {
            RequestStatus = requestStatus;
        }

        public RequestStatus RequestStatus { get; }

        public override string ToString()
        {
            return _converter.Convert(RequestStatus, null, null, null).ToString();
        }
    }
}
