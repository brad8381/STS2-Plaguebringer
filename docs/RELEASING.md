# Release process

## One repository

Keep source, useful code comments and developer documentation in this repository. Use feature branches and pull requests for review, and merge tested changes into `main`. Tag a tested commit for each release, for example `v0.1.0`.

A public repository's branches, commits and pull requests remain visible. A release package does not include those discussions. Development updates are kept in chat; GitHub is used for concise changes, reviews and actionable bugs.

## What players receive

Build and test the mod against the standard Steam branch and the declared BaseLib version. Create one ZIP containing a `Brad8381PlagueBringer/` folder with:

- `Brad8381PlagueBringer.dll`
- `Brad8381PlagueBringer.pck`
- `Brad8381PlagueBringer.json`

Players extract that folder into the game's `mods` folder and install BaseLib separately. Keep debug symbols separately for diagnosis. Do not bundle game DLLs or game asset packs.

Attach the same ZIP to GitHub Releases and Nexus Mods. Keep the version number and player-facing changelog identical. GitHub's automatically generated source ZIP is a separate download and is not the game package.

## Publishing to both sites

No publishing automation is configured yet.

Nexus's official [upload action](https://github.com/Nexus-Mods/upload-action) supports updates to an existing uploaded file. Initial setup requires a Nexus mod page and first uploaded file, then its file ID and an API key stored as a GitHub Actions secret. A future release workflow can upload the tested ZIP to GitHub and Nexus and supply the same changelog to both.

These are two separate uploads, not an atomic simultaneous publish: one may succeed while the other fails. Verify both results before announcing a release.

GitHub issues/PR comments and Nexus comments remain separate. Publish selected player-relevant fixes in the changelog; do not mirror development conversations.

Builds need access to the locally installed game assemblies and a matching Godot/MegaDot editor. A default GitHub-hosted runner does not include the game. Decide the build runner setup after the first successful local build.

## Release gate

- Compile and export successfully against the supported game build.
- Pass the relevant checks in [TESTING.md](TESTING.md).
- Record exact game and dependency versions.
- Review artwork, credits and the project's distribution license.
- Verify the ZIP contains only intended mod files.
- Publish only when the tested package is ready.

Retain the short AI-assistance disclosure in the project description. Voluntary support is allowed under [Mega Crit's content policy](https://www.megacrit.com/content-policy/); mod access and content remain free.
