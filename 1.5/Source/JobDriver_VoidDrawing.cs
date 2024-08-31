using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VAEInsanity
{
    public class JobDriver_VoidDrawing : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Log.Message("base.TargetA: " + base.TargetA);
            if (pawn.Reserve(base.TargetA, job, 1, -1, null, errorOnFailed))
            {
                return pawn.Reserve(base.TargetB, job, 1, -1, null, errorOnFailed);
            }
            return false;
        }

        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOn(() => pawn.MentalState is not MentalState_VoidDrawing);
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
            Toil toil = ToilMaker.MakeToil("MakeNewToils");
            toil.initAction = delegate
            {
                pawn.jobs.posture = PawnPosture.Standing;
            };
            toil.handlingFacing = true;
            toil.tickAction = delegate
            {
                pawn.rotationTracker.FaceCell(base.TargetB.Cell);
            };
            toil.AddFinishAction(delegate
            {
                Log.Message("base.TargetB; " + base.TargetB);
                FilthMaker.TryMakeFilth(base.TargetB.Cell, pawn.Map, DefsOf.VAEI_VoidDrawing);
                var state = pawn.MentalState as MentalState_VoidDrawing;
                state.SetNextDrawingTick();
            });
            toil.defaultCompleteMode = ToilCompleteMode.Delay;
            toil.defaultDuration = 300;
            yield return toil;
        }
    }
}
