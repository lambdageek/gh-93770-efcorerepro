#! /bin/bash

set -x
set -e

env_args=()
cli_args=()
program=""

for x in "$@"
do
    case "$x" in
    --debug)
        env_args+=("--debug");
        ;;
    --debugger-agent*)
        env_args+=("$x");
        ;; 
    *)
        if [[ z"$program" == z"" ]] ; then
            program="$x"
            continue
        fi
        cli_args+=("$x");
        ;;
    esac
done

if [[ z"$program" == z ]] ; then
    echo "No program specified"
    exit 1
fi

export MONO_ENV_OPTIONS="${MONO_ENV_OPTIONS} ${env_args[@]}"
exec "$program" "${cli_args[@]}"
