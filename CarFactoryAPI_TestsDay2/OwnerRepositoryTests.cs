using CarAPI.Entities;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactoryAPI_TestsDay2
{
    public class OwnerRepositoryTests
    {
        // Mock Dependencies
        Mock<FactoryContext> mockFactoryContext;
        // By Using Fake Object 
        OwnerRepository ownerRepository;
        public OwnerRepositoryTests() 
        {
            //mock dependencies
            mockFactoryContext = new();
            //use fake object 
            ownerRepository = new(mockFactoryContext.Object);
        }
        //---------------------------------------------------------------------------------
        [Fact]
        [Trait("About", "OwnerRepository")]
        public void GetAllOwners_AskForOwners_listOfOwners()
        {
            // Step1:arrange
            //Mock Data Building
            List<Owner> owners = new List<Owner>()
            {
                new Owner(){  Id = 1 ,Name="Moustafa"},
                new Owner(){  Id = 10 ,Name="Kamal"},
                new Owner(){  Id = 100 ,Name="Mohammed"}
            };
            //Calling DBSet By Stepup It 
            mockFactoryContext.Setup(o => o.Owners).ReturnsDbSet(owners);

            //Step2: act
            var result=ownerRepository.GetAllOwners();
            //Step3:assets
            Assert.NotEmpty(result);
        }
//------------------------------------------------------------------------------------------
        [Fact]
        [Trait("About", "OwnerRepository")]
        public void AddOwner_AddingOwner_true()
        {
            //arrange
            //Mock Data Building
            List<Owner> owners = new List<Owner>()
            {
                new Owner(){  Id = 1 ,Name="Moustafa"},
                new Owner(){  Id = 10 ,Name="Kamal"},
                new Owner(){  Id = 100 ,Name="Mohammed"}
            };
            Owner owner = new Owner() { Id = 1, Name = "Moustafa" };
            //Calling DBSet By Stepup It
            mockFactoryContext.Setup(o => o.Owners).ReturnsDbSet(owners);

            //Step2: act
            var result = ownerRepository.AddOwner(owner);
            //Step3:assets
            Assert.True(result);

        }
        //-----------------------------------------------------------------------------------
        [Fact(Skip ="understanding DPset in Mock")]
        [Trait("About", "OwnerRepository")]
        public void AddOwner_NotAddingOwnerAlreadyExit_False ()
        {
            //Step1: arrange
            //Mock Data Building
            List<Owner> owners = new List<Owner>()
            {
                new Owner(){  Id = 1 ,Name="Moustafa"},
                new Owner(){  Id = 10 ,Name="Kamal"},
                new Owner(){  Id = 100 ,Name="Mohammed"}
            };
            Owner owner = new Owner() { Id = 1, Name = "Moustafa" };
            //setup called Dbsets
            mockFactoryContext.Setup(o => o.Owners).ReturnsDbSet(owners);

            //Step2: act
            var result = ownerRepository.AddOwner(owner);
            //Step3:assets
            Assert.Equal(4, mockFactoryContext.Object.Owners.Count());
        }

    }
}
