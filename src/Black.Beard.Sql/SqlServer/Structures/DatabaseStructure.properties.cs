using System.ComponentModel;
using System.Xml.Linq;
using Bb.SqlServer.Structures.Dacpacs;

namespace Bb.SqlServer.Structures
{

    public partial class DatabaseStructure
    {

        public string DatabaseName { get; set; }

        [Category("")]
        [PropertySerialized("SET COMPATIBILITY_LEVEL = {0}")]
        public CompatibilityLevelEnum CompatibilityLevel { get; set; } = CompatibilityLevelEnum.SqlServer2019;

        [PropertySerialized("SET ANSI_NULL_DEFAULT {0}")]
        public OnOffEnum AnsiNullDefault { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET ANSI_NULLS {0}")]
        public OnOffEnum AnsiNulls { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET ANSI_PADDING {0}")]
        public OnOffEnum AnsiPadding { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET ANSI_WARNINGS {0}")]
        public OnOffEnum AnsiWarnings { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET ARITHABORT {0}")]
        public OnOffEnum Arithabort { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET AUTO_CLOSE {0}")]
        public OnOffEnum AutoClose { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET AUTO_SHRINK {0}")]
        public OnOffEnum AutoShrink { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET AUTO_UPDATE_STATISTICS {0}")]
        public OnOffEnum AutoUpdateStatistics { get; set; } = OnOffEnum.ON;

        [PropertySerialized("SET CURSOR_CLOSE_ON_COMMIT {0}")]
        public OnOffEnum CursorCloseOnCommit { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET CURSOR_DEFAULT {0}")]
        public CursorModeEnum CursorDefault { get; set; } = CursorModeEnum.GLOBAL;

        [PropertySerialized("SET CONCAT_NULL_YIELDS_NULL {0}")]
        public OnOffEnum ConcatNullYieldsNull { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET NUMERIC_ROUNDABORT {0}")]
        public OnOffEnum NumericRoundAbort { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET QUOTED_IDENTIFIER {0}")]
        public OnOffEnum QuotedIdentifier { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET RECURSIVE_TRIGGERS {0}")]
        public OnOffEnum RecursiveTriggers { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET AUTO_UPDATE_STATISTICS_ASYNC {0}")]
        public OnOffEnum AutoUpdateStatisticsAsync { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET DATE_CORRELATION_OPTIMIZATION {0}")]
        public OnOffEnum DateCorrelationOptimization { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET PARAMETERIZATION {0}")]
        public ParametrizationModeEnum Parametrization { get; set; } = ParametrizationModeEnum.SIMPLE;

        [PropertySerialized("SET READ_COMMITTED_SNAPSHOT {0}")]
        public OnOffEnum ReadCommitedSnapShot { get; set; } = OnOffEnum.OFF;

        [PropertySerialized("SET {0}")]
        public DatabaseReadModeEnum DatabaseReadOnly { get; set; } = DatabaseReadModeEnum.READ_WRITE;

        [PropertySerialized("SET RECOVERY {0}")]
        public RecoveryModeEnum Recovery { get; set; } = RecoveryModeEnum.FULL;

        [PropertySerialized("SET {0}")]
        public RestrictAccessEnum RestrictAccess { get; set; } = RestrictAccessEnum.MULTI_USER;

        [PropertySerialized("SET PAGE_VERIFY {0}")]
        public PageVerifyEnum PageVerify { get; set; } = PageVerifyEnum.CHECKSUM;

        [PropertySerialized("SET TARGET_RECOVERY_TIME = {0} SECONDS")]
        public int TargetRecoveryTimeInSecond { get; set; } = 60;

        [PropertySerialized("SET DELAYED_DURABILITY {0}")]
        public DelayedDurabilityEnum DelayedDurability { get; set; } = DelayedDurabilityEnum.DISABLED;

        //[PropertySerialized("SET AUTO_CREATE_STATISTICS {0}(INCREMENTAL = OFF)")]
        //public OnOffEnum AutoCreateStatistics { get; set; } = OnOffEnum.ON;

        [PropertySerialized("SET {0}")]
        public BrokerEnum Broker { get; set; } = BrokerEnum.DISABLE_BROKER;



        [PropertySerialized("SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = {0};")]
        public OnOff2Enum CardinalyEstimation { get; set; } = OnOff2Enum.Off;

        [PropertySerialized("SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = {0};")]
        public OnOffPrimaryEnum CardinalyEstimationForSecondary { get; set; } = OnOffPrimaryEnum.Primary;

        [PropertySerialized("SCOPED CONFIGURATION SET MAXDOP = {0};")]
        public int MaxDop { get; set; } = 0;

        //[PropertySerialized("SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = {0};")]
        //public int MaxDopSecondary { get; set; } = 0;


        [PropertySerialized("SCOPED CONFIGURATION SET PARAMETER_SNIFFING = {0};")]
        public OnOff2Enum ParameterSniffing { get; set; } = OnOff2Enum.On;

        [PropertySerialized("SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = {0};")]
        public OnOffPrimaryEnum ParameterSniffingForSecondary { get; set; } = OnOffPrimaryEnum.Primary;


        [PropertySerialized("SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = {0};")]
        public OnOff2Enum QueryOptimizerHotfixes { get; set; } = OnOff2Enum.Off;

        [PropertySerialized("SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = {0};")]
        public OnOffPrimaryEnum QueryOptimizerHotfixesForSecondary { get; set; } = OnOffPrimaryEnum.Primary;

        public ContainmentEnum ContainmentType { get; set; } = ContainmentEnum.NONE;


    }



    public enum CompatibilityLevelEnum
    {

        [PropertySerialized("100")]
        SqlServer2008 = 10,

        [PropertySerialized("110")]
        SqlServer2012 = 11,

        [PropertySerialized("120")]
        SqlServer2014 = 12,

        [PropertySerialized("130")]
        SqlServer2016 = 13,

        [PropertySerialized("140")]
        SqlServer2017 = 14,

        [PropertySerialized("150")]
        SqlServer2019 = 15,
    }

    public enum ContainmentEnum
    {
        NONE,
        PARTIAL
    }

    public enum OnOffEnum
    {
        OFF,
        ON
    }

    public enum OnOffPrimaryEnum
    {
        Off,
        On,
        Primary
    }

    public enum OnOff2Enum
    {
        Off,
        On
    }

    public enum CursorModeEnum
    {
        GLOBAL,
        LOCAL
    }

    public enum BrokerEnum
    {
        DISABLE_BROKER,
        ENABLE_BROKER
    }

    public enum ParametrizationModeEnum
    {
        SIMPLE,
        FORCED
    }

    public enum RestrictAccessEnum
    {
        MULTI_USER,
        SINGLE_USER,
        RESTRICTED_USER
    }

    public enum DatabaseReadModeEnum
    {
        READ_WRITE,
        READ_ONLY
    }

    public enum RecoveryModeEnum
    {
        FULL,
        BULK_LOGGED,
        SIMPLE
    }

    public enum PageVerifyEnum
    {
        CHECKSUM,
        TORN_PAGE_DETECTION,
        NONE
    }

    public enum DelayedDurabilityEnum
    {
        DISABLED,
        ALLOWED,
        FORCED
    }

}