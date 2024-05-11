namespace Tests.Test.Objects
{
    public static class TestLaunchPopulatedBaseEntity
    {
        public static Launch Test1()
        {
            var test1 = new Launch()
            {
                Id = new Guid("000ebc80-d782-4dee-8606-1199d9074039"),
                AtualizationDate = DateTime.Now,
                ImportedT = DateTime.Now,
                EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                ApiGuid = new Guid("bb9c0abd-767f-4844-8e47-d02252d3415b"),
                Status = new Status()
                {
                    Id = new Guid("3932b460-1d53-4533-b960-33cbc62a1f4b"),
                    IdFromApi = 3,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                },
                IdLaunchServiceProvider = new Guid("c9c15036-f45a-4109-95ec-fdc387fe7733"),
                LaunchServiceProvider = new LaunchServiceProvider()
                {
                    Id = new Guid("c9c15036-f45a-4109-95ec-fdc387fe7733"),
                    IdFromApi = 66,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                },
                IdRocket = new Guid("6735c96e-a46e-4fef-942c-af827e04572d"),
                Rocket = new Rocket()
                {
                    Id = new Guid("6735c96e-a46e-4fef-942c-af827e04572d"),
                    IdFromApi = 3628,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdConfiguration = new Guid("a1fea86a-874f-4bfa-8317-821b200ee6e7"),
                    Configuration = new Configuration()
                    {
                        Id = new Guid("a1fea86a-874f-4bfa-8317-821b200ee6e7"),
                        IdFromApi = 327,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    }
                },
                IdMission = new Guid("b7c69fdc-32fb-4ad6-8722-c8a864a3d61d"),
                Mission = new Mission()
                {
                    Id = new Guid("b7c69fdc-32fb-4ad6-8722-c8a864a3d61d"),
                    IdFromApi = 2046,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdOrbit = new Guid("94d7a867-f918-4ad8-b93b-e0f108d66f2d"),
                    Orbit = new Orbit()
                    {
                        Id = new Guid("94d7a867-f918-4ad8-b93b-e0f108d66f2d"),
                        IdFromApi = 8,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    }
                },
                IdPad = new Guid("a578ae03-a75a-45fa-9d9b-ddfee3c7a4e3"),
                Pad = new Pad()
                {
                    Id = new Guid("a578ae03-a75a-45fa-9d9b-ddfee3c7a4e3"),
                    IdFromApi = 139,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdLocation = new Guid("6600d3fc-f421-4e88-8509-3b9f3e69939e"),
                    Location = new Location()
                    {
                        Id = new Guid("6600d3fc-f421-4e88-8509-3b9f3e69939e"),
                        IdFromApi = 139,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    }
                }
            };
        
            return test1;
        }
    
        public static Launch Test2()
        {
            var test2 = new Launch()
            {
                Id = new Guid("001f670e-a06c-49dc-8797-777171f45263"),
                AtualizationDate = DateTime.Now,
                ImportedT = DateTime.Now,
                EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                ApiGuid = new Guid("8ce5a4c6-8ab4-40e1-af08-924da819d4b1"),
                IdStatus = new Guid("e9ae28ee-3202-4b73-a3fa-30f3156d1d38"),
                Status = new Status()
                {
                    Id = new Guid("e9ae28ee-3202-4b73-a3fa-30f3156d1d38"),
                    IdFromApi = 4,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                },
                IdLaunchServiceProvider = new Guid("26f4868c-c973-4d9a-9423-ece20371bc7e"),
                LaunchServiceProvider = new LaunchServiceProvider()
                {
                    Id = new Guid("26f4868c-c973-4d9a-9423-ece20371bc7e"),
                    IdFromApi = 161,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                },
                IdRocket = new Guid("cfb7e247-646c-4349-a422-8742bb4668e1"),
                Rocket = new Rocket()
                {
                    Id = new Guid("cfb7e247-646c-4349-a422-8742bb4668e1"),
                    IdFromApi = 3937,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdConfiguration = new Guid("2c6195be-ae6a-40ab-943f-079ffc75c7b0"),
                    Configuration = new Configuration()
                    {
                        Id = new Guid("2c6195be-ae6a-40ab-943f-079ffc75c7b0"),
                        IdFromApi = 408,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    }
                },
                IdMission = new Guid("18e959e6-21cc-4b0c-b9cd-65c95ee91598"),
                Mission = new Mission()
                {
                    Id = new Guid("18e959e6-21cc-4b0c-b9cd-65c95ee91598"),
                    IdFromApi = 2355,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdOrbit = new Guid("4cc3a31f-5602-4156-975b-06a6bcc645ad"),
                    Orbit = new Orbit()
                    {
                        Id = new Guid("4cc3a31f-5602-4156-975b-06a6bcc645ad"),
                        IdFromApi = 6,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    }
                },
                IdPad = new Guid("3f1eac5b-1496-4308-8e94-0e0888a56bb9"),
                Pad = new Pad()
                {
                    Id = new Guid("3f1eac5b-1496-4308-8e94-0e0888a56bb9"),
                    IdFromApi = 14,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdLocation = new Guid("89d4177c-08b2-431f-a258-d7187d9db97c"),
                    Location = new Location()
                    {
                        Id = new Guid("89d4177c-08b2-431f-a258-d7187d9db97c"),
                        IdFromApi = 12,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    }
                }
            };
       
            return test2;
        }
    
        public static Launch Test3()
        {
            var test3 = new Launch()
            {
                Id = new Guid("0022751c-b755-4d0c-a23a-294ce9c95c71"),
                AtualizationDate = DateTime.Now,
                ImportedT = DateTime.Now,
                EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                ApiGuid = new Guid("d849ffac-babd-4c86-a61c-3ec6b0927a0a"),
                IdStatus = new Guid("3932b460-1d53-4533-b960-33cbc62a1f4b"),
                Status = new Status()
                {
                    Id = new Guid("3932b460-1d53-4533-b960-33cbc62a1f4b"),
                    IdFromApi = 3,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                },
                IdLaunchServiceProvider = new Guid("c9c15036-f45a-4109-95ec-fdc387fe7733"),
                LaunchServiceProvider = new LaunchServiceProvider()
                {
                    Id = new Guid("c9c15036-f45a-4109-95ec-fdc387fe7733"),
                    IdFromApi = 66,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                },
                IdRocket = new Guid("d86d7bc6-30ce-4baa-b4ac-4d46e9303b06"),
                Rocket = new Rocket()
                {
                    Id = new Guid("d86d7bc6-30ce-4baa-b4ac-4d46e9303b06"),
                    IdFromApi = 5153,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdConfiguration = new Guid("d80ee7ed-b1ff-498e-805f-a8a5b11d38e1"),
                    Configuration = new Configuration()
                    {
                        Id = new Guid("d80ee7ed-b1ff-498e-805f-a8a5b11d38e1"),
                        IdFromApi = 37,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    }
                },
                IdMission = new Guid("5ebb423d-988a-4645-a719-ed75491dd41f"),
                Mission = new Mission()
                {
                    Id = new Guid("5ebb423d-988a-4645-a719-ed75491dd41f"),
                    IdFromApi = 3564,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdOrbit = new Guid("94d7a867-f918-4ad8-b93b-e0f108d66f2d"),
                    Orbit = new Orbit()
                    {
                        Id = new Guid("94d7a867-f918-4ad8-b93b-e0f108d66f2d"),
                        IdFromApi = 8,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    }
                },
                IdPad = new Guid("42796995-8d7e-4bf2-8f34-6d739c967204"),
                Pad = new Pad()
                {
                    Id = new Guid("42796995-8d7e-4bf2-8f34-6d739c967204"),
                    IdFromApi = 85,
                    ImportedT = DateTime.Now,
                    AtualizationDate = DateTime.Now,
                    EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    IdLocation = new Guid("e12c85df-3353-44f9-b728-331a33ef8032"),
                    Location = new Location()
                    {
                        Id = new Guid("e12c85df-3353-44f9-b728-331a33ef8032"),
                        IdFromApi = 6,
                        ImportedT = DateTime.Now,
                        AtualizationDate = DateTime.Now,
                        EntityStatus = EStatus.PUBLISHED.GetDisplayName(),
                    }
                }
            };
        
            return test3;
        }
    }
}