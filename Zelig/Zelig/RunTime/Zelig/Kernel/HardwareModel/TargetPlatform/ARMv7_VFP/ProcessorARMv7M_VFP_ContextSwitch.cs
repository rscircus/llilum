﻿//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

//#define DEBUG_CTX_SWITCH

using System.Runtime.InteropServices;

namespace Microsoft.Zelig.Runtime.TargetPlatform.ARMv7
{
    using System;

    using EncDef = Microsoft.Zelig.TargetModel.ArmProcessor.EncodingDefinition;
    using TS     = Microsoft.Zelig.Runtime.TypeSystem;
    using RT     = Microsoft.Zelig.Runtime;


    [TS.DisableAutomaticReferenceCounting]
    public abstract partial class ProcessorARMv7M_VFP 
    {
        //--//

        //
        // Part of Context may be defined in the model for the targeted sub-system, e.g. Mbed or CMSIS-Core for ARM processors
        //

        [TS.DisableAutomaticReferenceCounting]
        public abstract unsafe new class Context : Processor.Context
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct SoftwareFrame
            {
                // SW stack frame: pushed by PendSV_Handler
                // SW stack frame 
                [TS.AssumeReferenced] public uint    EXC_RETURN;
                [TS.AssumeReferenced] public uint    CONTROL;
                [TS.AssumeReferenced] public UIntPtr R4;
                [TS.AssumeReferenced] public UIntPtr R5;
                [TS.AssumeReferenced] public UIntPtr R6;
                [TS.AssumeReferenced] public UIntPtr R7;
                [TS.AssumeReferenced] public UIntPtr R8;
                [TS.AssumeReferenced] public UIntPtr R9;
                [TS.AssumeReferenced] public UIntPtr R10;
                [TS.AssumeReferenced] public UIntPtr R11;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct SoftwareFloatingPointFrame
            {
                // SW stack frame for FP               
                [TS.AssumeReferenced] public float   S16;
                [TS.AssumeReferenced] public float   S17;
                [TS.AssumeReferenced] public float   S18;
                [TS.AssumeReferenced] public float   S19;
                [TS.AssumeReferenced] public float   S20;
                [TS.AssumeReferenced] public float   S21;
                [TS.AssumeReferenced] public float   S22;
                [TS.AssumeReferenced] public float   S23;
                [TS.AssumeReferenced] public float   S24;
                [TS.AssumeReferenced] public float   S25;
                [TS.AssumeReferenced] public float   S26;
                [TS.AssumeReferenced] public float   S27;
                [TS.AssumeReferenced] public float   S28;
                [TS.AssumeReferenced] public float   S29;
                [TS.AssumeReferenced] public float   S30;
                [TS.AssumeReferenced] public float   S31;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct HardwareFrame
            {
                // HW stack frame: pushed upon entering PendSV_Handler
                [TS.AssumeReferenced] public UIntPtr R0;
                [TS.AssumeReferenced] public UIntPtr R1;
                [TS.AssumeReferenced] public UIntPtr R2;
                [TS.AssumeReferenced] public UIntPtr R3;
                [TS.AssumeReferenced] public UIntPtr R12;
                [TS.AssumeReferenced] public UIntPtr LR;
                [TS.AssumeReferenced] public UIntPtr PC;
                [TS.AssumeReferenced] public UIntPtr PSR;

                // HW stack frame for FP
                [TS.AssumeReferenced] public float   S0;
                [TS.AssumeReferenced] public float   S1;
                [TS.AssumeReferenced] public float   S2;
                [TS.AssumeReferenced] public float   S3;
                [TS.AssumeReferenced] public float   S4;
                [TS.AssumeReferenced] public float   S5;
                [TS.AssumeReferenced] public float   S6;
                [TS.AssumeReferenced] public float   S7;
                [TS.AssumeReferenced] public float   S8;
                [TS.AssumeReferenced] public float   S9;
                [TS.AssumeReferenced] public float   S10;
                [TS.AssumeReferenced] public float   S11;
                [TS.AssumeReferenced] public float   S12;
                [TS.AssumeReferenced] public float   S13;
                [TS.AssumeReferenced] public float   S14;
                [TS.AssumeReferenced] public float   S15;
                [TS.AssumeReferenced] public UIntPtr FPSCR_1;
                [TS.AssumeReferenced] public UIntPtr FPSCR_2;
            }

            //[TS.WellKnownType( "Microsoft_Zelig_ProcessorARMv4_RegistersOnStack" )]
            [StructLayout(LayoutKind.Sequential)]
            public struct RegistersOnStackFullFPContext 
            {
                public const uint StackRegister          = EncDef.c_register_sp;
                public const uint LinkRegister           = EncDef.c_register_lr;
                public const uint ProgramCounterRegister = EncDef.c_register_pc;

                //
                // State
                //


                public SoftwareFrame              SoftwareFrameRegisters;
                public SoftwareFloatingPointFrame SoftwareFloatingPointFrame;
                public HardwareFrame              HardwareFrameRegisters;

                //
                // Helper Methods
                //

                public static unsafe uint TotalFrameSize
                {
                    [RT.Inline]
                    get
                    {
                        return HWFrameSize + SwitcherFrameSize;
                    }
                }

                public static unsafe uint HWFrameSize
                {
                    [RT.Inline]
                    get
                    {
                        return (uint)sizeof(HardwareFrame);
                    }
                }

                public static unsafe uint SwitcherFrameSize
                {
                    [RT.Inline]
                    get
                    {
                        return (uint)(sizeof(SoftwareFrame) + sizeof(SoftwareFloatingPointFrame));
                    }
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct RegistersOnStackNoFPContext
            {
                public const uint StackRegister = EncDef.c_register_sp;
                public const uint LinkRegister = EncDef.c_register_lr;
                public const uint ProgramCounterRegister = EncDef.c_register_pc;

                //
                // State
                //

                public SoftwareFrame SoftwareFrameRegisters;
                public HardwareFrame HardwareFrameRegisters;

                //
                // Helper Methods
                //

                public static unsafe uint TotalFrameSize
                {
                    [RT.Inline]
                    get
                    {
                        return HWFrameSize + SwitcherFrameSize;
                    }
                }

                public static unsafe uint HWFrameSize
                {
                    [RT.Inline]
                    get
                    {
                        return (uint)sizeof(HardwareFrame);
                    }
                }

                public static unsafe uint SwitcherFrameSize
                {
                    [RT.Inline]
                    get
                    {
                        return (uint)sizeof(SoftwareFrame);
                    }
                }
            }

            //--//

            //
            // This is the pointer to the last known position of the stack pointer
            // For a long jump this points to the end of the SW context to install
            //
            protected UIntPtr SP;
            protected uint    EXC_RETURN;
            protected bool    m_isFullContext;

            //--//

            //
            // Overrides
            //

            public override void SwitchTo( )
            {
                // The long jump selects the current thread's context and sets its EXC_RETURN value
                ProcessorARMv7M.RaiseSupervisorCall( ProcessorARMv7M.SVC_Code.SupervisorCall__LongJump );
                
#if DEBUG_CTX_SWITCH
                BugCheck.Log( "!!!!!!!!!!!!!!!!!!!!!  ERROR  !!!!!!!!!!!!!!!!!!!!!!!" );
                BugCheck.Log( "!!! Back after Long Jump after, Ctx Switch Failed !!!" );
                BugCheck.Log( "!!!!!!!!!!!!!!!!!!!!!  ERROR  !!!!!!!!!!!!!!!!!!!!!!!" );
#endif

                RT.BugCheck.Assert( false, BugCheck.StopCode.IllegalMode );
            }
            
            public override void Populate( )
            {
                BugCheck.Raise( BugCheck.StopCode.InvalidOperation );
            }

            public override void Populate( Processor.Context context )
            {
                BugCheck.Raise( BugCheck.StopCode.InvalidOperation );
            }

            public unsafe override void PopulateFromDelegate( Delegate dlg, uint[] stack )
            {
                DelegateImpl dlgImpl   = (DelegateImpl)(object)dlg;
                ArrayImpl    stackImpl = (ArrayImpl   )(object)stack;
                ObjectImpl   objImpl   = (ObjectImpl  )(object)dlg.Target;

                //
                // Save the initial stack pointer
                // In the general case the SP will be at the top of the current frame we are building
                // When we do a LongJump though, or we start the thread first, we will have to use the base stack pointer
                //
                this.SP         = GetFirstStackPointerFromPhysicalStack( stackImpl );
                this.EXC_RETURN = c_MODE_RETURN__THREAD_PSP;

                //
                // Initial offset from start of stack storage must be at least as large as a frame
                //
                RT.BugCheck.Assert(
                    ((int)stackImpl.GetEndDataPointer() - this.SP.ToUInt32()) >= RegistersOnStackNoFPContext.TotalFrameSize,
                    BugCheck.StopCode.StackCorruptionDetected);

                //
                // build the first stack frame
                //
                RegistersOnStackNoFPContext* firstFrame = PointerToSimpleFrame(this.SP);

                firstFrame->HardwareFrameRegisters.PC         = new UIntPtr( dlgImpl.InnerGetCodePointer().Target.ToPointer() );
                firstFrame->HardwareFrameRegisters.R0         = objImpl.ToPointer();
                firstFrame->HardwareFrameRegisters.PSR        = new UIntPtr(ProcessorARMv7M.c_psr_InitialValue);
                firstFrame->SoftwareFrameRegisters.EXC_RETURN = c_MODE_RETURN__THREAD_PSP;   // !!! here we assume that no context starts with FP context active !!!
                firstFrame->SoftwareFrameRegisters.CONTROL    = c_CONTROL__MODE__THRD_PRIV;

#if DEBUG_CTX_SWITCH
                RT.BugCheck.Log( "[PFD-ctx] EXC=0x%08x, PSR=0x%08x, PC=0x%08x, R0=0x%08x, SP(aligned)=0x%08x",
                    (int)firstFrame->EXC_RETURN,
                    (int)firstFrame->PSR.ToUInt32( ),
                    (int)firstFrame->PC.ToUInt32( ),
                    (int)firstFrame->R0.ToUInt32( ),
                    (int)this.SP.ToUInt32( )
                    );

                RT.BugCheck.Log( "[PFD-stackImpl] SP(start)=0x%08x, SP(end)=0x%08x, SP(length)=0x%08x, SP(offset)=0x%08x",
                    (int)( stackImpl.GetDataPointer( ) ),
                    (int)( stackImpl.GetEndDataPointer( ) ),
                    (int)( stackImpl.GetEndDataPointer( ) - stackImpl.GetDataPointer( ) ),
                    (int)( (int)stackImpl.GetEndDataPointer( ) - this.SP.ToUInt32( ) )
                    );
#endif
            }

            public override void SetupForExceptionHandling( uint mode )
            {
                //
                // Stop any exception from happening
                //
                using(Runtime.SmartHandles.InterruptState.DisableAll())
                {
                    //
                    // Retrieve the MSP< which we will use to handle exceptions
                    //
                    UIntPtr stack = ProcessorARMv7M.GetMainStackPointer();
                    
                    ////
                    //// Enter target mode, with interrupts disabled.
                    ////                    
                    //SwitchToHandlerPrivilegedMode( );

                    //
                    // Set the stack pointer in the context to be the current MSP
                    //
                    this.StackPointer = stack;
                    
                    ////
                    //// Switch back to original mode
                    ////                    
                    //SwitchToThreadUnprivilegedMode( ); 
                }
            }
            
#region Tracking Collector and Exceptions  

            public override bool Unwind( )
            {
                throw new Exception( "Unwind not implemented" );
            }

            public override unsafe UIntPtr GetRegisterByIndex( uint idx )
            {
                //return *( this.Registers.GetRegisterPointer( idx ) );
                return (UIntPtr)0;
            }

            public override unsafe void SetRegisterByIndex( uint idx, UIntPtr value )
            {
                //*( this.Registers.GetRegisterPointer( idx ) ) = value;
            }

            #endregion

            private static UIntPtr ContextSwitch( ThreadManager tm, UIntPtr stackPointer, bool isFullFrame )
            {
                ThreadImpl currentThread = tm.CurrentThread;
                ThreadImpl nextThread    = tm.NextThread;
                Context    ctx;

                if(currentThread != null)
                {
                    ctx = (Context)currentThread.SwappedOutContext;

                    //
                    // update SP as well as the EXC_RETURN address
                    //     
                    ctx.IsFullContext = isFullFrame;
                    ctx.EXC_RETURN = isFullFrame 
                        ? PointerToFullFrame(stackPointer)->SoftwareFrameRegisters.EXC_RETURN
                        : PointerToSimpleFrame(stackPointer)->SoftwareFrameRegisters.EXC_RETURN;
                    ctx.StackPointer = stackPointer;
                }

                ctx = (Context)nextThread.SwappedOutContext;

                //
                // Pass EXC_RETURN down to the native portion of the 
                // PendSV handler we need to offset to the beginning of the frame
                //
                SetExcReturn( ctx.EXC_RETURN );

                //
                // Update thread manager state and Thread.CurrentThread static field
                //
                tm.CurrentThread = nextThread;

                ThreadImpl.CurrentThread = nextThread;

                return ctx.StackPointer;
            }

            //--//
            //--//
            //--//
            
            private static unsafe void FirstLongJump( )
            {
                LongJump( ); 
            }
            
            private static unsafe void LongJump( )
            {
                //
                // Retrieve next context from ThreadManager
                //
                Context currentThreadCtx = (ProcessorARMv7M_VFP.Context)ThreadManager.Instance.CurrentThread.SwappedOutContext;

                //
                // Set the PSP at R0 so that returning from the SVC handler will complete the work
                //
                SetProcessStackPointer( AddressMath.Increment( 
                    currentThreadCtx.StackPointer, 
                    currentThreadCtx.IsFullContext 
                        ? RegistersOnStackFullFPContext.SwitcherFrameSize 
                        : RegistersOnStackNoFPContext.SwitcherFrameSize ) );

                SetExcReturn( currentThreadCtx.EXC_RETURN );

                //
                // SWitch to unprivileged mode before jumping to our thread 
                // This can only be enabled when we have a model for allowing tasks 
                // to enable/disable interrupts
                //
                //ProcessorARMv7M.SwitchToUnprivilegedMode( ); 
            }

            private static unsafe void LongJumpForRetireThread( )
            {
                LongJump( ); 
            }
            
            private unsafe UIntPtr GetFirstStackPointerFromPhysicalStack( ArrayImpl stackImpl )
            {
                UIntPtr addressOfStackFrame = AddressMath.Decrement(new UIntPtr(stackImpl.GetEndDataPointer()), RegistersOnStackNoFPContext.TotalFrameSize);
                return AddressMath.AlignToLowerBoundary(addressOfStackFrame, 8);
            }

            //
            // Access Methods
            //

            public override UIntPtr StackPointer
            {
                [RT.Inline]
                get { return this.SP; }
                [RT.Inline]
                set { this.SP = value; }
            }

            public bool IsFullContext
            {
                [RT.Inline]
                get { return this.m_isFullContext; }
                [RT.Inline]
                set { this.m_isFullContext = value; }
            }

            public override UIntPtr ProgramCounter
            {
                get
                {
                    //return Registers.PC;
                    return (UIntPtr)0;
                }
                set
                {
                    //Registers.PC = value;
                }
            }

            public override uint ScratchedIntegerRegisters
            {
                get { return 0; }
            }

            //--//
            
            [RT.Inline]
            internal static unsafe Context.RegistersOnStackFullFPContext* PointerToFullFrame( UIntPtr SP )
            {
                return (Context.RegistersOnStackFullFPContext*)SP.ToPointer( );
            }
            [RT.Inline]
            internal static unsafe Context.RegistersOnStackNoFPContext* PointerToSimpleFrame( UIntPtr SP )
            {
                return (Context.RegistersOnStackNoFPContext*)SP.ToPointer( );
            }

            [RT.Inline]
            private static unsafe Context.RegistersOnStackFullFPContext* GetFullFrameFromPhysicalStack( )
            {
                return (Context.RegistersOnStackFullFPContext*)GetProcessStackPointer( ).ToPointer( );
            }

            //
            // Helpers 
            //

            [Inline]
            public static void InterruptHandlerWithContextSwitch( ref RegistersOnStackNoFPContext registers )
            {
                Peripherals.Instance.ProcessInterrupt( );

#if USE_HARDWARE_INTERRUPT_VECTOR
                ThreadManager tm = ThreadManager.Instance;

                //
                // We keep looping until the current and next threads are the same,
                // because when swapping out a dead thread, we might wake up a different thread.
                //
                while(tm.ShouldContextSwitch)
                {
                    ContextSwitch( tm, ref registers );
                }
#endif
            }

            [Inline]
            public static void InterruptHandlerWithoutContextSwitch( )
            {
                Peripherals.Instance.ProcessInterrupt( );
            }

            [Inline]
            public static void FastInterruptHandlerWithoutContextSwitch( )
            {
                Peripherals.Instance.ProcessFastInterrupt( );
            }

            [Inline]
            public static void GenericSoftwareInterruptHandler( ref RegistersOnStackNoFPContext registers )
            {
            }

            //--//

            [Inline]
            private static unsafe void PrepareStackForException( uint mode, RegistersOnStackNoFPContext* ptr )
            {

                //
                // EXC_RETURN to go back to previous mode.
                //
                // TODO: 

                //
                // Save unbanked R4 -R11.
                //


                // TODO: 

                //
                // Switch back to the exception handling mode.
                //


                // TODO: 

                //
                // R1 should point to the Register Context on the stack.
                //

                // TODO: 
                //SetRegister( EncDef.c_register_r1, new UIntPtr( ptr ) );
            }

            [Inline]
            private static unsafe void RestoreStackForException( uint mode, RegistersOnStackNoFPContext* ptr )
            {

                //
                // EXC_RETUNR to go back to previous mode.
                //
                // TODO: 

                //
                // Save unbanked R4 -R11.
                //


                // TODO: 

                //
                // Switch back to the exception handling mode.
                //


                // TODO: 

                //
                // R1 should point to the Register Context on the stack.
                //

                // TODO: 
                //SetRegister( EncDef.c_register_r1, new UIntPtr( ptr ) );
            }
            
            //
            // All overridable exceptions for Ctx Switch
            //
            
            [RT.HardwareExceptionHandler( RT.HardwareException.Interrupt )]
            [RT.ExportedMethod]
            private static unsafe void SVC_Handler_Zelig_VFP_NoFPContext( uint* args )
            {
                SVC_Code svc_number = (SVC_Code)((byte*)args[6])[-2]; // svc number is at stacked PC offset - 2 bytes

                switch(svc_number)
                {
                    case SVC_Code.SupervisorCall__LongJump:
                        LongJump( ); 
                        break;
                    case SVC_Code.SupervisorCall__StartThreads:
                        FirstLongJump( );
                        break;
                    case SVC_Code.SupervisorCall__RetireThread:
                        LongJumpForRetireThread( );
                        break;
                    default:
                        BugCheck.Assert( false, BugCheck.StopCode.Impossible );
                        break;
                }
            }
            
            
            [RT.HardwareExceptionHandler( RT.HardwareException.Interrupt )]
            [RT.ExportedMethod]
            private static UIntPtr PendSV_Handler_Zelig_VFP( UIntPtr stackPointer, uint isParitalStack )
            {
                using (SmartHandles.InterruptState.Disable( ))
                {
                    return ContextSwitch( ThreadManager.Instance, stackPointer, isParitalStack == 0 );
                }
            }
            
            [RT.HardwareExceptionHandler( RT.HardwareException.Interrupt )]
            [RT.ExportedMethod]
            private static void AnyInterrupt( )
            {

            }        
        }
    }
}
