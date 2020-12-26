namespace Monito.Domain.Entity
{
    public class RequestOptionsDomainEntity {
        public virtual JobHttpMethod Method { get; private set; }
        public virtual int Redirects { get; private set; }
        public virtual int Threads { get; private set; }
        public virtual int Timeout { get; private set; }
        public virtual string UserAgent { get; private set; }

        public static RequestOptionsDomainEntity Build(
            JobHttpMethod method,
            int redirects,
            int threads,
            int timeout,
            string userAgent
        ) {
            // TODO: Validate redirects, threads, timeout, useragent
            return new RequestOptionsDomainEntity() {
                Method = method,
                Redirects = redirects,
                Threads = threads,
                Timeout = timeout,
                UserAgent = userAgent
            };
        }
    }

    public enum JobHttpMethod
    {
        GET = 1,
        HEAD = 2,
    }
}