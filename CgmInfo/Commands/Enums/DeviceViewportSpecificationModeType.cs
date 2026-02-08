namespace CgmInfo.Commands.Enums
{
    public enum DeviceViewportSpecificationModeType
    {
        [TextToken("FRACTION")]
        FractionOfDrawingSurface = 0,
        [TextToken("MM")]
        MillimetersWithScaleFactor = 1,
        [TextToken("PHYDEVCOORD")]
        PhysicalDeviceCoordinates = 2,
    }
}
