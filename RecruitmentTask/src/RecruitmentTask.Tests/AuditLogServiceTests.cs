using AutoFixture;
using FluentAssertions;
using Moq;
using RecruitmentTask.Application.Services.AuditLogs;
using RecruitmentTask.Domain.AuditLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentTask.Tests;

public class AuditLogServiceTests
{
	private readonly AuditLogService _sut;

	private readonly Mock<IAuditLogRepository> _mockAuditLogRepository;

	public AuditLogServiceTests()
	{
		_sut = new AuditLogService(_mockAuditLogRepository.Object);

		_mockAuditLogRepository = new Mock<IAuditLogRepository>();
	}

	[Fact]
	public async Task GetAuditLogsAsync_ShouldReturnCorrectData_WhenEntityTypeIsNotContractHeaderEntity()
	{
		// Arrange
		var fixture = new Fixture();
		var organizationId = Guid.NewGuid();
		int page = 1;
		int pageSize = 10;

		var mockAuditLogs = new List<AuditLog>
		{
			fixture.Build<AuditLog>()
				.With(a => a.OrganizationId, organizationId)
				.With(a => a.EntityType, EntityType.FileEntity)
                .With(a => a.ContractNumber, (string)null)
                .Create()
		};

		_mockAuditLogRepository.Setup(r => r.GetAuditLogsAsync(organizationId, page, pageSize))
							   .ReturnsAsync(mockAuditLogs);

		// Act
		var result = await _sut.GetAuditLogsAsync(organizationId, page, pageSize);

		// Assert
		result.Should().NotBeNull();
		result.Should().ContainSingle();
		var auditLog = result.First();
		auditLog.ContractNumber.Should().BeNull();
	}

	[Fact]
	public async Task GetAuditLogsAsync_ShouldReturnCorrectData_WhenEntityTypeIsContractHeaderEntity()
	{
		// Arrange
		var fixture = new Fixture();
		var organizationId = Guid.NewGuid();
		int page = 1;
		int pageSize = 10;
		string contractNumber = "12345";

		var mockAuditLogs = new List<AuditLog>
		{
			fixture.Build<AuditLog>()
				.With(a => a.OrganizationId, organizationId)
				.With(a => a.EntityType, EntityType.ContractHeaderEntity)
                .With(a => a.ContractNumber, contractNumber)
                .Create()
		};

		_mockAuditLogRepository.Setup(r => r.GetAuditLogsAsync(organizationId, page, pageSize))
							   .ReturnsAsync(mockAuditLogs);

		// Act
		var result = await _sut.GetAuditLogsAsync(organizationId, page, pageSize);

		// Assert
		result.Should().NotBeNull();
		result.Should().ContainSingle();
		var auditLog = result.First();
		auditLog.ContractNumber.Should().Be(contractNumber);
	}


	[Fact]
	public async Task GetAuditLogsAsync_ShouldThrowArgumentException_WhenOrganizationIdIsEmpty()
	{
		// Arrange
		var invalidOrganizationId = new Guid();
		int page = 1;
		int pageSize = 10;

		// Act & Assert
		var act = async () => await _sut.GetAuditLogsAsync(invalidOrganizationId, page, pageSize);
		await act.Should().ThrowAsync<ArgumentException>().WithMessage("Organization id must not be empty.");
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public async Task GetAuditLogsAsync_ShouldThrowArgumentException_WhenPageIsLessThanOrEqualToZero(int invalidPage)
	{
		// Arrange
		var organizationId = Guid.NewGuid();
		int pageSize = 10;

		// Act & Assert
		var act = async () => await _sut.GetAuditLogsAsync(organizationId, invalidPage, pageSize);
		await act.Should().ThrowAsync<ArgumentException>().WithMessage("Page must be greater than zero.");
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public async Task GetAuditLogsAsync_ShouldThrowArgumentException_WhenPageSizeIsLessThanOrEqualToZero(int invalidPageSize)
	{
		// Arrange
		var organizationId = Guid.NewGuid();
		int page = 10;

		// Act & Assert
		var act = async () => await _sut.GetAuditLogsAsync(organizationId, page, invalidPageSize);
		await act.Should().ThrowAsync<ArgumentException>().WithMessage("Page size must be greater than zero.");
	}

	[Fact]
	public async Task GetAuditLogsAsync_ShouldReturnEmptyCollection_WhenNoAuditLogsFound()
	{
		// Arrange
		var organizationId = Guid.NewGuid();
		var page = 1;
		var pageSize = 10;

		_mockAuditLogRepository.Setup(r => r.GetAuditLogsAsync(organizationId, page, pageSize))
							   .ReturnsAsync(Enumerable.Empty<AuditLog>());

		// Act
		var result = await _sut.GetAuditLogsAsync(organizationId, page, pageSize);

		// Assert
		result.Should().BeEmpty();
	}
}