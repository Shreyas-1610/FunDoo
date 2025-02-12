using CommonLayer.Models;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interface
{
    public interface INotesManager
    {
        public Notes CreateNote(int UserId, NotesModel notesModel);
        public List<Notes> GetAllNotes(int UserId);
        public bool UpdateNote(int NotesId, int UserId, UpdateNoteModel updateNoteModel);
        public bool DeleteNote(int NotesId, int UserId);
        public bool CheckPin(int NotesId, int UserId);
        public bool CheckArchive(int NotesId, int UserId);
        public bool CheckTrash(int NotesId, int UserId);
        public string UpdateColor(int NotesId, int UserId, string Color);
        public bool UpdateImage(int NotesId, int UserId, string ImagePath);
        public bool UpdateReminder(int NotesId, int UserId, DateTime reminder);

    }
}
