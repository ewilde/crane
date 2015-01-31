# Introduction
Thank you for deciding to help out with `crane`. Contributions are really appreciated. We are still in the early stages of building version 1.0 of `crane` and would love to get feedback and direction from others.

This document explains how to contribute changes to `crane`. Hopefully you will agree it's quite straight-forward.

# Make a change in 5 simple steps
1. [Create an issue](https://github.com/ewilde/crane/issues) to talk about the change. Take a look at the [waffle board](https://waffle.io/ewilde/crane) to see what's going on and how the releases look.
2. Fork [crane](https://github.com/ewilde/crane/fork)

3. clone crane 
`git clone https://github.com/ewilde/crane.git`

4. makes changes based off the `develop` branch and commit back to your `crane` repository

5. Send us a pull request


# Branches
1. `master` is for releasing 
We don't accept pull request on the master branch. At the end of the current milestone a *crane* collaborator will merge develop into master. This initiates the master build to run on [teamcity](http://teamcity.cranebuild.com/viewType.html?buildTypeId=crane_Master&branch_crane=%3Cdefault%3E&tab=buildTypeStatusDiv) and a new package to get pushed to chocolatey

2. `develop` 
We would love to receive pull requests on this branch. All tests must pass before merging into develop.

3. `feature-[short-name]-[issue-no]`
When working on features we tend to branch off develop into a feature branch and merge back into develop when that feature is complete and all the tests pass
