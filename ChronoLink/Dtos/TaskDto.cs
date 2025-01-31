  namespace ChronoLink.Dtos{
  
  public class TaskDto
    {
        public object Id { get; set; }
        public object Description { get; set; }
        public object StartDateTime { get; set; }
        public object EndDateTime { get; set; }
        public object WorkspaceId { get; set; }
        public object AssignedUserId { get; set; }
        public object AssignedUserName { get; set; }
    }

}