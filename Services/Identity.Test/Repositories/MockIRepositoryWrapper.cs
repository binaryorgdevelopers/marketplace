using Moq;

namespace Identity.Test.Repositories;

internal class MockIRepositoryWrapper
{
    public static Mock<IRepositoryWrapper> GetMock()
    {
        var mock = new Mock<IRepositoryWrapper>();
        var userRepository = MockIUserRepository.GetMock();

        mock.Setup(m => m.User).Returns(() => userRepository.Object);

        return mock;
    }
}