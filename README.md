# TPOT Seeder :seedling:

## Quickstart :spiral_note_pad:

To install everything to run this API, run the following from GitBash, Bash or `sh`ell:

`bash install-packages.sh`

Then run it in Hot Reload Mode by running `dotnet watch -p .`.  This is similar to `nodemon`, but much, much faster. :wink:

Swagger will generate a test UI for you and will auto regenerate on changes.

> NOTE: If you try changing any variables marked `static`, you won't induce a change.  Why?  Because static variables are cached on the heap and it's my suspicion that only stack memory can be Hot Reloaded.  This is ok because it's predictable and makes sense from the language design