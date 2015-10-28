namespace PhotoContest.App.Services
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v2;
    using Google.Apis.Services;

    public class GoogleDriveService
    {
        private const string GoogleDriveServiceAccountEmail = "275150299950-ce8pimu7o0jl2cctvkbhjqaovjsb4csk@developer.gserviceaccount.com";

        private const string GoogleDrivePrivateKey =
            "-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCb5MYtl2VpTUYg\nBSSwqsi9hQplx2LwF54takWeZUy4ajgXDU7dhe7AqW869+oOSQcsvnYIfS8oqAB1\nfS3mKsBNysnBGDAIBX9JSEhq8rdKJI8MdfLsU4NR1kLZNENpNABz62Ra7d0PW4h2\ntm/AF3ixh84KagMr3Laey27bhygOAnrOawXRWGs0hO/4hFybnkeCPKPGH76BFklh\ncGif0hdq/dPIbklEjqEgD2rz2zzCzAM7hMan2lVvYMZmEAhRAG9RexdWf38801Zu\nznuKpz25vZzPM5WAM0ITKLq3ynBy0q5cA3w11QtlaT7VMvLJXwDrg0yoxYmCqKZZ\no1ptHTzdAgMBAAECggEAQocfC9XQMWAIzSFkxwAbKsXb6hNs5Ykut7LigvY4B3tM\n9Il5XpAJk667CS9Dc1U2+qFNPdIujeskRv9k1xTnfEtOTllEJigyadOvE/UAw2NW\nqLqtMK1zHTmSzZ7AJeVLTCzoZuWbsTIeyoqQpileGGUcSNV1BQLr7Fhktsq3DULX\nGpuKE4WfaMKJ0f6pk4pJbEEPhH3TgLx7eldi+BjCaxWdj2CGu6VELkjLaBdBMQY3\nOWqsQq0HCvO48cfvt22pE9aN48YzFV5993YgLxH0Q7jG896que2TYqiOQqZHSya2\nQSc5rGnrVxRaHfBMbvsunas0WNtldEgj3wCor0x4AQKBgQDL3uYw3nAvKAhdJyDU\nXU832rK3YslDmXSGn8TTNkcx/pk0FDodAgJ2qUaOMLLfpYgM2HAWW2Yz8RzWLkzT\nLFA62cQkjTLAJuNZ7q5CU6MFJrCXUdc1NSftMbM1zYUKUlYY8/GqIQrOD3gVuzlu\nbmcAYa7eC4npi3AZGkOMqRJDpQKBgQDDwVypKwBfW9irO+3TD44QU6vwcBqB8p2x\n2hoIrPM4ktYxO4tBrVkzqHW8GF53ZmyZ1QEDEaQUF8vPp6UGSL1hj7x/buabGUqc\nOe/4b2Ggpo5jZZWBdZYMF2QVNx4O+WblTEQ/csetzXNYrYEJol4/lCNaEtZLdxiy\nYZpQjH1u2QKBgQCSqeE56NwV4Jvbv+lrPVFfIjMNoMCfpAbjTo7/T336Zml6wM5p\nzedNzEtYwYn9QI1WiGJigxBMb7nKD3bvOGLjY4Qqs3V+LLvEF+Q/DQIrE4FY6uSC\nf6Igjm+ZVifOnBsaSmxhcvHd+dzu7UjMvBnXeW6eiPiHDevv8ygDSiI8jQKBgD6N\ndVyhXxcZtFzTY6XEHLNe6VyFlhLcQkPo29TV30q+sS8+RbuQ9yYouKaIavdghWeK\nxy4B3xqEmB4dBgDCEbIxShy2hX4eUNcnNGWAwgOh9XIRrfqWZoIqn1KgMDy8uYKg\nIlbcCK6jLi6yEr/PYKqUXc+UoWGwBvZJdhQzMInRAoGBAI6UbrESNBvh1dIjKH2e\n+pKVcGKh3GJgRlXJ8JQZYzGYLITIKflt0+dIR6yGyAK8ZkE7VxX/6wc7FLEY79MY\nEh9zYHLyvzpVXx8A7g/l6JEqgju2CBFpONtJUQ2/gIFVF1wf6R7rpJvynPLEP/v1\nvE772sjEYK3DsTdhBVCwV0Bx\n-----END PRIVATE KEY-----\n";

        public static DriveService Get()
        {
            string[] scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };

            var credentials =
                new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(GoogleDriveServiceAccountEmail) { Scopes = scopes }.FromPrivateKey(GoogleDrivePrivateKey));

            return new DriveService(new BaseClientService.Initializer { HttpClientInitializer = credentials });
        }
    }
}