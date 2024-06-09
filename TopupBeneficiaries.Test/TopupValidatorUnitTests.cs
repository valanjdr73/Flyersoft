using Moq;
using TopupBeneficiaries.Services;
using TopupBeneficiaries.Entities;
using TopupBeneficiaries.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using TopupBeneficiaries.Test.Fixtures;

namespace TopupBeneficiaries.Test;

public class TopupValidatorUnitTests : IClassFixture<TopupValidatorFixture>
{
    private readonly TopupValidatorFixture _fixture;
    public TopupValidatorUnitTests(TopupValidatorFixture topupValidatorFixture)
    {
        _fixture = topupValidatorFixture;
    }

    [Fact]
    public void ValidateVerifiedUserTopup_Topup_WithNoTransactionRecords()
    {    
        bool returnValue = _fixture.topupValidatior.ValidateVerifiedUserTopup(new User(),
            _fixture.TopUpBeneficiary, _fixture.TopUpOption,
            null);

        Assert.True( returnValue
            );

         returnValue = _fixture.topupValidatior.ValidateVerifiedUserTopup(new User(),
    _fixture.TopUpBeneficiary, _fixture.TopUpOption,
    new List<TopUpTransaction>());

        Assert.True(returnValue);

    }

    [Fact]
    public void ValidateVerifiedUserTopup_Topup_WithTransactionDataWithinLimit()
    {
        //VerifiedUser Limit is set at 200

        _fixture.TransactionList.Clear();
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary, TopUpBeneficiaryId= _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption, TopUpOptionId = _fixture.TopUpOption.Id
        });
        bool returnValue = _fixture.topupValidatior.ValidateVerifiedUserTopup(new User(),
           _fixture.TopUpBeneficiary, _fixture.TopUpOption,
           _fixture.TransactionList);

        Assert.True(returnValue
            );
    }

    [Fact]
    public void ValidateVerifiedUserTopup_Topup_WithTransactionDataNotWithinLimit()
    {

        //VerifiedUser Limit is set at 200

        _fixture.TransactionList.Clear();
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });

        bool returnValue = _fixture.topupValidatior.ValidateVerifiedUserTopup(new User(),
           _fixture.TopUpBeneficiary, _fixture.TopUpOption,
           _fixture.TransactionList);

        Assert.False(returnValue
            );

    }


    [Fact]
    public void ValidateUnVerifiedUserTopup_Topup_WithNoTransactionRecords()
    {
        bool returnValue = _fixture.topupValidatior.ValidateUnVerifiedUserTopup(new User(),
            _fixture.TopUpBeneficiary, _fixture.TopUpOption,
            null);

        Assert.True(returnValue
            );

        returnValue = _fixture.topupValidatior.ValidateUnVerifiedUserTopup(new User(),
   _fixture.TopUpBeneficiary, _fixture.TopUpOption,
   new List<TopUpTransaction>());

        Assert.True(returnValue);

    }

    [Fact]
    public void ValidateUnVerifiedUserTopup_Topup_WithTransactionDataWithinLimit()
    {
        //VerifiedUser Limit is set at 200

        _fixture.TransactionList.Clear();
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        bool returnValue = _fixture.topupValidatior.ValidateUnVerifiedUserTopup(new User(),
           _fixture.TopUpBeneficiary, _fixture.TopUpOption,
           _fixture.TransactionList);

        Assert.True(returnValue
            );
    }

    [Fact]
    public void ValidateUnVerifiedUserTopup_Topup_WithTransactionDataNotWithinLimit()
    {

        //VerifiedUser Limit is set at 200

        _fixture.TransactionList.Clear();
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });

        bool returnValue = _fixture.topupValidatior.ValidateUnVerifiedUserTopup(new User(),
           _fixture.TopUpBeneficiary, _fixture.TopUpOption,
           _fixture.TransactionList);

        Assert.False(returnValue
            );

    }

    [Fact]
    public void ValidateUserTopupLimitTopup_WithNoTransactionRecords()
    {
        bool returnValue = _fixture.topupValidatior.ValidateUnVerifiedUserTopup(new User(),
            _fixture.TopUpBeneficiary, _fixture.TopUpOption,
            null);

        Assert.True(returnValue
            );

        returnValue = _fixture.topupValidatior.ValidateUnVerifiedUserTopup(new User(),
   _fixture.TopUpBeneficiary, _fixture.TopUpOption,
   new List<TopUpTransaction>());

        Assert.True(returnValue);

    }

    [Fact]
    public void ValidateUserTopupLimit_Topup_WithTransactionDataWithinLimit()
    {
        //VerifiedUser Limit is set at 500

        _fixture.TransactionList.Clear();
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        bool returnValue = _fixture.topupValidatior.ValidateUnVerifiedUserTopup(new User(),
           _fixture.TopUpBeneficiary, _fixture.TopUpOption,
           _fixture.TransactionList);

        Assert.True(returnValue
            );
    }

    [Fact]
    public void ValidateUserTopupLimit_Topup_WithTransactionDataNotWithinLimit()
    {

        //VerifiedUser Limit is set at 500

        _fixture.TransactionList.Clear();
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });
        _fixture.TransactionList.Add(new TopUpTransaction()
        {
            TopUpBeneficiary = _fixture.TopUpBeneficiary,
            TopUpBeneficiaryId = _fixture.TopUpBeneficiary.Id,
            TopUpOption = _fixture.TopUpOption,
            TopUpOptionId = _fixture.TopUpOption.Id
        });

        bool returnValue = _fixture.topupValidatior.ValidateUnVerifiedUserTopup(new User(),
           _fixture.TopUpBeneficiary, _fixture.TopUpOption,
           _fixture.TransactionList);

        Assert.False(returnValue
            );

    }

}
