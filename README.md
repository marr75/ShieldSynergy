# ShieldSynergy

A BepInEx 5 (Harmony) plugin for **Solar Expanse** that makes stacking radiation
shields on a facility superlinear instead of vanilla's flat per-unit sum:

- **Superlinear stacking** — the radiation-reduction bonus from `n` enabled shield
  units scales as `perUnit x n^p` instead of `perUnit x n`, so building more shields
  on one facility pays off faster than vanilla's linear stack.
- **Configurable exponent** — `ShieldSynergyExponent` (`p`, default `1.5`) controls
  how superlinear the bonus is; `1.0` reproduces vanilla behavior.
- **Optional safety cap** — `ShieldSynergyMaxMultiplier` clamps the applied
  multiplier to bound pathological shield stacks (`0` = uncapped).
- **Master toggle** — `ShieldSynergyEnabled` early-returns the patch, leaving
  radiation byte-identical to vanilla when off. All three settings are read live,
  so tuning takes effect without a rebuild.

Plugin GUID: `marr75.solarexpanse.shieldsynergy`

## Build

1. Set the `SOLAR_EXPANSE_DIR` environment variable to your Solar Expanse install path
   (the folder containing `Solar Expanse_Data\Managed`).
2. Run `dotnet build`.

The post-build `DeployToPlugins` target copies the DLL to
`%SOLAR_EXPANSE_DIR%\BepInEx\plugins\ShieldSynergy\`, so a successful build is a deployed build.

Override the path per-build without the env var via `dotnet build -p:GameDir="..."`.

## License

License: MIT (see LICENSE)
