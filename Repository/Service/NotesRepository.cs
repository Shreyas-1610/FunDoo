using CommonLayer.Models;
using Repository.Context;
using Repository.Entity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Service
{
    public class NotesRepository : INotesRepository
    {
        private readonly FunDooDbContext context;
        public NotesRepository(FunDooDbContext context)
        {
            this.context = context;
        }
        public Notes CreateNote(int UserId, NotesModel notesModel)
        {
            Notes notes = new Notes();
            notes.Title = notesModel.Title;
            notes.Description = notesModel.Description;
            notes.CreatedAt = DateTime.Now;
            notes.UpdatedAt = DateTime.Now;
            notes.UserId = UserId;

            context.Notes.Add(notes);
            context.SaveChanges();
            return notes;
        }

        public List<Notes> GetAllNotes(int UserId)
        {
            var userNotes = context.Notes.Where(n => n.UserId == UserId && !n.IsTrash).ToList();

            if (userNotes.Count() > 0)
            {
                return userNotes;
            }

            throw new Exception("No notes found for this user");
        }

        public bool UpdateNote(int NotesId,int UserId, UpdateNoteModel updateNoteModel)
        {
            var record = context.Notes.FirstOrDefault(n=>n.NotesId == NotesId);
            if (record == null)
            {
                return false;
            }
            record.Reminder = updateNoteModel.Reminder;
            record.Color = updateNoteModel.Color;
            record.Image = updateNoteModel.Image;
            record.IsArchive = updateNoteModel.IsArchive;
            record.IsPin = updateNoteModel.IsPin;
            record.IsTrash = updateNoteModel.IsTrash;
            record.UpdatedAt = DateTime.Now;

            context.SaveChanges();
            return true;
        }

        public bool DeleteNote(int NotesId, int UserId)
        {
            var record = context.Notes.FirstOrDefault(n=>n.NotesId == NotesId && n.UserId == UserId);
            if(record == null)
            {
                return false;
            }
            context.Notes.Remove(record);
            context.SaveChanges();
            return true;
        }

        public bool CheckPin(int NotesId, int UserId)
        {
            var note = context.Notes.FirstOrDefault(n=>n.NotesId == NotesId && n.UserId == UserId);
            if(note == null)
            {
                return false;
            }
            note.IsPin = !note.IsPin;
            note.UpdatedAt = DateTime.Now;

            context.SaveChanges();
            return true;
        }

        public bool CheckArchive(int NotesId, int UserId)
        {
            var note = context.Notes.FirstOrDefault(n => n.NotesId == NotesId && n.UserId == UserId);
            if (note == null)
            {
                return false; 
            }
            note.IsArchive = !note.IsArchive;
            note.UpdatedAt = DateTime.Now;

            context.SaveChanges();
            return true;
        }

        public bool CheckTrash(int NotesId, int UserId)
        {
            var note = context.Notes.FirstOrDefault(n => n.NotesId == NotesId && n.UserId == UserId);
            if (note == null)
            {
                return false;
            }
            note.IsTrash = !note.IsTrash;
            note.UpdatedAt = DateTime.Now;

            context.SaveChanges();
            return true;
        }

        public string UpdateColor(int NotesId, int UserId, string Color)
        {
            var note = context.Notes.FirstOrDefault(n => n.NotesId == NotesId && n.UserId == UserId);
            if (note == null)
            {
                return "No note found";
            }
            note.Color = Color;
            note.UpdatedAt = DateTime.Now;

            context.SaveChanges();
            return Color;
        }

        public bool UpdateImage(int NotesId, int UserId, string ImagePath)
        {
            var record = context.Notes.FirstOrDefault(i => i.NotesId == NotesId && i.UserId == UserId);
            if (record == null)
            {
                return false;
            }
            record.Image = ImagePath;
            record.UpdatedAt = DateTime.Now;

            context.SaveChanges();
            return true;
        }

        public bool UpdateReminder(int NotesId, int UserId, DateTime reminder)
        {
            var record = context.Notes.FirstOrDefault(i => i.NotesId == NotesId && i.UserId == UserId);
            if (record == null)
            {
                return false;
            }
            record.Reminder = reminder;
            record.UpdatedAt = DateTime.Now;

            context.SaveChanges();
            return true;
        }
    }
}
