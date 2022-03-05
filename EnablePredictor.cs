// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Management.Automation;
using System.Management.Automation.Subsystem;
using System.Management.Automation.Subsystem.Prediction;

namespace Megasware128.Predictor
{
    [Cmdlet("Enable", "CarapacePredictor")]
    [OutputType(typeof(void))]
    public class EnablePredictor : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            SubsystemManager.RegisterSubsystem<ICommandPredictor, CarapacePredictor>(new CarapacePredictor());
        }
    }
}