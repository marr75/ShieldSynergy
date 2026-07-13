using ShieldSynergy.Config;

namespace ShieldSynergy.Core;

static class Services {
    internal static Configuration Config { get; private set; } = null!;
    internal static void Init(Configuration config) { Config = config; }
}
