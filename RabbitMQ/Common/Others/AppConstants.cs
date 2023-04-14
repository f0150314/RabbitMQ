namespace Common.Others;

public static class AppConstants
{
    public const string AppSettingFile = "appsettings.json";
    public const string RoutingKey_Rpc = "remote_procedure_call";
    public const string RoutingKey_Queue = "task_queue";
    public const string RoutingKey_Direct = "direct";
    public const string RoutingKey_Topic = "topic.#";
    public const string ExchangeKey_Fanout = "fanout";
    public const string ExchangeKey_Direct = "direct";
    public const string ExchangeKey_Topic = "topic";
}