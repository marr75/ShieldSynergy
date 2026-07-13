using BepInEx.Configuration;

namespace ShieldSynergy.Config;

sealed class Configuration {
    public readonly ConfigEntry<bool> Enabled;
    public readonly ConfigEntry<double> StackingCurveStrength;
    public readonly ConfigEntry<double> StackingBonusCap;

    public Configuration(ConfigFile c) {
        const string enabledDescription = "Turns the stacking bonus on or off. Off = shields work exactly like unmodified Solar "
            + "Expanse (flat linear stacking). Read live, so toggling takes effect without a rebuild.";
        Enabled = c.Bind(
            "ShieldSynergy",
            "Enabled",
            true,
            enabledDescription
        );
        const string stackingCurveStrengthDescription = "How much extra reward you get for stacking shields together. 1.0 = vanilla (no bonus, "
            + "each shield just adds). Above 1.0, each additional shield is worth more than the last "
            + "-- a curve of 2.0 on a stack of 4 shields protects noticeably more than 4x a single "
            + "shield. The game's own habitability math tapers off at high radiation reduction, so "
            + "pushing this very high mostly buys diminishing real-world payoff even though the math "
            + "keeps climbing. Read live.";
        StackingCurveStrength = c.Bind(
            "ShieldSynergy",
            "StackingCurveStrength",
            1.5,
            new ConfigDescription(
                stackingCurveStrengthDescription,
                new AcceptableValueRange<double>(0.5, 10.0)
            )
        );
        const string stackingBonusCapDescription = "Optional safety ceiling on the stacking bonus, so a big shield cluster combined with a "
            + "steep curve above can't multiply your shielding without limit. 0 = uncapped (the bonus "
            + "grows as far as the curve above takes it). Dev/advanced knob -- most players can leave "
            + "this at 0. Read live.";
        StackingBonusCap = c.Bind(
            "Debug",
            "StackingBonusCap",
            0.0,
            new ConfigDescription(
                stackingBonusCapDescription,
                new AcceptableValueRange<double>(0.0, 100.0)
            )
        );
    }
}
