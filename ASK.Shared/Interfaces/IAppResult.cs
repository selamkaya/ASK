namespace ASK.Shared.Interfaces;

public interface IAppResult
{
	List<string> Messages { get; set; }

	bool Succeeded { get; set; }
}

public interface IAppResult<out TResult> : IAppResult
{
	TResult Data { get; }
}