namespace FlatStage;

public static class Random
{
    public static readonly DefaultRandomGenerator Default = new();
    public static readonly GaussianRandomGenerator Gaussian = new();
}