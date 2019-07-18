using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shopalooza.WebUI.Tests.Mocks
{
    public class HttpMockContext : HttpContextBase
    {
        private MockResponse _mockResponse;
        private MockRequest _mockRequest;
        private HttpCookieCollection _cookies;

        public HttpMockContext()
        {
            _cookies = new HttpCookieCollection();
            _mockRequest = new MockRequest(_cookies);
            _mockResponse = new MockResponse(_cookies);
        }

        public override HttpRequestBase Request
        {
            get
            {
                return _mockRequest;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return _mockResponse;
            }
        }

    }

    public class MockResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection _cookies;

        public MockResponse(HttpCookieCollection cookies)
        {
            _cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }
    }

    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection _cookies;

        public MockRequest(HttpCookieCollection cookies)
        {
            _cookies = cookies; 
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }
    }
}
