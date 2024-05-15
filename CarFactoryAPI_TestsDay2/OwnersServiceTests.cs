using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Payment;
using CarAPI.Repositories_DAL;
using CarAPI.Services_BLL;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace CarFactoryAPI_TestsDay2
{
    public class OwnersServiceTests:IDisposable
    {
        private readonly ITestOutputHelper testOutputHelper;
        // Mock Dependencies
        Mock<ICarsRepository> mockCarRepo;
        Mock<IOwnersRepository> mockOwnerRepo;
        Mock<ICashService> mockCashService;
        
        // By Using Fake Object  
        OwnersService ownersService;
        public OwnersServiceTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            testOutputHelper.WriteLine("test setup here");
            //mock dependencies
            mockCarRepo = new();
            mockOwnerRepo = new();
            mockCashService= new();
            //use fake object 
            ownersService =new OwnersService(mockCarRepo.Object, mockOwnerRepo.Object,mockCashService.Object);

        }

        public void Dispose()
        {
            testOutputHelper.WriteLine("test clean up here");  
        }
        //------------------------------------------------------------------------------------
        [Fact]
        [Trait("About","BuyCar")]

        public void BuyCar_OwnerHasCar_True()
        {
            //Step1:arrange
            //Mock Data Building
            Car car = new Car()
            {
                Id= 10,

            };
            Owner owner = new Owner()
            {
                Id= 1,  
                Car=new Car() { Id=5,}
            };
            //setup called methods
            BuyCarInput buyCarInput = new BuyCarInput()
            {
                CarId = 10,
                OwnerId = 1,
                Amount = 100000,
            };

            mockCarRepo.Setup(o=> o.GetCarById(10)).Returns(car);
            mockOwnerRepo.Setup(o=>o.GetOwnerById(1)).Returns(owner);
            //Step2:act
            string result = ownersService.BuyCar(buyCarInput);
            //Step2:assert
            Assert.Contains("have car", result);
        }
        //-----------------------------------------------------------------------------------
        [Fact]
        [Trait("About", "BuyCar")]

        public void BuyCar_OwnerInsufficientfunds_True()
        {
            //Step1:arrange
            //Mock Data Building
            Car car = new Car()
            {
                Id = 10,
                Price=200000

            };
            Owner owner = new Owner()
            {
                Id = 1,
            };
            //setup called methods
            BuyCarInput buyCarInput = new BuyCarInput()
            {
                CarId = 10,
                OwnerId = 1,
                Amount = 100000,
            };

            mockCarRepo.Setup(o => o.GetCarById(10)).Returns(car);
            mockOwnerRepo.Setup(o => o.GetOwnerById(1)).Returns(owner);
            //Step2:act
            string result = ownersService.BuyCar(buyCarInput);
            //Step3:assert
            Assert.Contains("Insufficient", result);
        }

        //-----------------------------------------------------------------------------------
        [Fact]
        [Trait("About", "BuyCar")]

        public void BuyCar_DataBaseError_True()
        {
            //Step1:arrange
            //Mock Data Building
            Car car = new Car()
            {
                Id = 10,
                Price = 200000

            };
            Owner owner = new Owner()
            {
                Id = 1,
            };
            //setup called methods
            BuyCarInput buyCarInput = new BuyCarInput()
            {
                CarId = 10,
                OwnerId = 1,
                Amount = 300000,
            };

            mockCarRepo.Setup(o => o.GetCarById(10)).Returns(car);
            mockCarRepo.Setup(o => o.AssignToOwner(10,1)).Returns(false);
            mockOwnerRepo.Setup(o => o.GetOwnerById(1)).Returns(owner);
            mockCashService.Setup(o => o.Pay(car.Price)).Returns($"Amount: {car.Price} is paid through Cash");

            //Step2:act
            string result = ownersService.BuyCar(buyCarInput);
            //Step3:assert
            Assert.Contains("went wrong", result);
        }
        //-----------------------------------------------------------------------------------
        [Fact]
        [Trait("About", "BuyCar")]

        public void BuyCar_OwenerBuyingCar_True()
        {
            //Step :arrange
            //Mock Data Building
            Car car = new Car()
            {
                Id = 10,
                Price = 200000

            };
            Owner owner = new Owner()
            {
                Id = 1,
            };
            //setup called methods
            BuyCarInput buyCarInput = new BuyCarInput()
            {
                CarId = 10,
                OwnerId = 1,
                Amount = 300000,
            };

            mockCarRepo.Setup(o => o.GetCarById(10)).Returns(car);
            mockCarRepo.Setup(o => o.AssignToOwner(10, 1)).Returns(true);
            mockOwnerRepo.Setup(o => o.GetOwnerById(1)).Returns(owner);
            mockCashService.Setup(o => o.Pay(car.Price)).Returns($"Amount: {car.Price} is paid through Cash");

            //Step: act
            string result = ownersService.BuyCar(buyCarInput);
            //Step3:assert
            Assert.Contains("Successfull", result);
        }



    }
}
