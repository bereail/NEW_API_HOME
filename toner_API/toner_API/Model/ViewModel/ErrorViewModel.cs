﻿namespace toner_API.Model.ViewModel
{
        public class ErrorViewModel
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string RequestId { get; set; }

            public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        }

}
