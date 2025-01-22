using UxCarrier.Data;
using UxCarrier.Repository.IRepository;

namespace UxCarrier.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _repoContext;
        private readonly EIVO03DbContext _eivoRepoContext;
        private IUxCardEmailRepository? _memberEmail;
        private IUxCardRepository? _member;
        private IUxBindCardRepository? _bindCard;
        private IEInvoiceBindCardRepository? _einovicebindCard;
        private IInvoiceRepository? _inoviceItem;
        public RepositoryWrapper(ApplicationDbContext repo, EIVO03DbContext eivoRepo)
        {
            _repoContext = repo;
            _eivoRepoContext = eivoRepo;
        }

        public IInvoiceRepository InvoiceItem
        {
            get
            {
                if (_inoviceItem == null)
                {
                    _inoviceItem = new InvoiceItemRepository(_eivoRepoContext);
                }
                return _inoviceItem;
            }
        }

        public IEInvoiceBindCardRepository EInvoiceBindCard
        {
            get
            {
                if (_einovicebindCard == null)
                {
                    _einovicebindCard = new EInvoiceBindCardRepository(_repoContext);
                }
                return _einovicebindCard;
            }
        }

        public IUxCardEmailRepository CardEmail
        {
            get
            {
                if (_memberEmail == null)
                {
                    _memberEmail = new UxMemberEmailRepository(_repoContext);
                }
                return _memberEmail;
            }
        }

        public IUxCardRepository Card
        {
            get
            {
                if (_member == null)
                {
                    _member = new UxMemberRepository(_repoContext);
                }
                return _member;
            }
        }

        public IUxBindCardRepository BindCard
        {
            get
            {
                if (_bindCard == null)
                {
                    _bindCard = new UxBindCardRepository(_repoContext);
                }
                return _bindCard;
            }
        }

        public void Save()
        {
            if (_repoContext.ChangeTracker.HasChanges())
                _repoContext.SaveChanges();
            else
                _eivoRepoContext.SaveChanges();
        }
    }
}
