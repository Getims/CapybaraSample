using Main.Scripts.Configs.Tasks;
using Main.Scripts.Core.Constants;
using Main.Scripts.Infrastructure.Providers.Assets;

namespace Main.Scripts.Infrastructure.Providers.Configs
{
    public interface ITasksConfigProvider
    {
        TasksGroupConfig Config { get; }
    }

    public class TasksConfigProvider : ITasksConfigProvider
    {
        public TasksGroupConfig Config { get; }

        public TasksConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<TasksGroupConfig>(ConfigsPaths.TASKS_CONFIG_PATH);
    }
}