namespace dirty_tpot_links_seeder;

public interface ITPOTPaperRepository
{
	void InsertTPOTPaper(TPOTPaper TPOTPaper);
	IList<TPOTPaper> GetTPOTPaperByType(string type);
	void UpdateTPOTPaperList(IList<TPOTPaper> TPOTPaperList);
}
