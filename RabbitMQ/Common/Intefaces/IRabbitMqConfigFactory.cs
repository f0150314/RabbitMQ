namespace Common.Intefaces;

public interface IRabbitMqConfigFactory<T> where T : IConfig
{
    T CreateConfig();
}