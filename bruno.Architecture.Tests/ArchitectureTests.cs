using FluentAssertions;
using NetArchTest.Rules;

namespace bruno.Architecture.Tests
{
    public class ArchitectureTests
    {
        private const string DomainNamespace = "bruno.Domain";
        private const string ApplicationNamespace = "bruno.Application";
        private const string InfrastructureNamespace = "bruno.Infrastructure";
        private const string ContractsNamespace = "bruno.Contracts";
        private const string WebApiNamespace = "bruno.WebApi";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange 
            var assembly = typeof(Domain.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                ApplicationNamespace,
                InfrastructureNamespace,
                ContractsNamespace,
                WebApiNamespace
            };

            // Act
            var testResult = Types.InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }
    }
}