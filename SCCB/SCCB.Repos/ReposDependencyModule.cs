using Autofac;
using SCCB.Repos.UnitOfWork;

namespace SCCB.Repos
{
    public class ReposDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork.UnitOfWork>().As<IUnitOfWork>();
        }
    }
}
