namespace MachineryProcessingDemo.helper
{
    public enum ProductDocType
    {
        Normal  = 1 , 
        Scrap = 2 
    }
    

    public enum OfflineType
        {
            Normal=1 , 
            NG = 2 ,
            Repair = 3 , 
            Scrap = 4  ,
            Restruct = 5 ,
            Concession =6 
        }

        public enum ProcedureFirstStatus
        {
            Qualified = 1 ,
            Concession = 2  ,
            Repair = 3 ,
            Scrap = 4 , 
            NG = 5 
        }

        public enum CheckTaskState
        {
            NotOnline = 1 , 
            InExecution = 2 ,
            Completed = 3
        }

        public enum CheckReason
        {
            First = 1, 
            LastPieceNotGood = 2 , 
            MeetFrequency = 3, 
            Repair = 4 , 
            LastProcedure = 5 , 
            Bad = 6
        }

        public enum CheckType
        {
            SelfCheck = 1 , 
            ThreeCoordinate = 2, 
            Manual = 3, 
            RawMaterialOpen = 4 , 
            OutSourcingOpen = 5 , 
            SquareMaterialOpen = 6
        }
        public enum ProcedureType
        {
            Tooling = 5 , 
            Machining = 1,
            Bench = 2,
            Outsourcing = 3,
            HotPressing = 4
        }

        public enum ApsProcedureTaskState
        {
            WaitPublish =0,
            ToDo = 1,
            InExecution = 2,
            Completed = 3,
        }

        public enum ApsProcedureTaskDetailState
        {
            NotOnline = 1,
            InExecution = 2,
            Completed = 3,
            Repair = 4 , 
            ForceDown = 5
        }

        public enum ProductProcessingOfflineType
        {
            Normal = 1 , 
            Force = 2 , 
            Bad = 3 
        }

        public enum ProductProcessingOnlineType
        {
            Normal = 1 , 
            Repair = 2 
        }
        public enum PositionType
        {
            Machining = 1,
            Stock = 2
        }

        public enum PlanProductInfoState
        {
            NotOnline = 0 , 
            InExcecution = 1 , 
            Completed = 2 
        }

        public enum CstateState
        {
            NoAndWaitToConfirm = 0 , 
            NoAndOutofDate = 1 , 
            Yes  = 2 , 

        }

        public enum QDOrProdutcProOnlineType
        {
            Normal = 1 , 
            Repair = 2 
        }
}
