<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"><xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_credittermupdcustomer">
    <table class="tblDesc">
	  <tr>
			<td class="colW1">{lbl:GroupID}</td>
			<td class="colW2">
				<xsl:value-of select="@groupid" />
			</td>
			<td class="colW1">{lbl:CreditGrade}</td>
			<td class="colW2">
				<xsl:value-of select="@creditgrade" />
			</td>
			<td class="colW1">{lbl:PreAgreeAmount}</td>
			<td class="colW2">
				<xsl:value-of select="@preagreeamount" />
			</td>
	  </tr>
	  <tr>
			<td class="colW1">{lbl:InstallmentAmount}</td>
			<td class="colW2">
			  <xsl:value-of select="@installmentamount" />
			</td>
			<td class="colW1">{lbl:InstallmentDate}</td>
			<td class="colW2">
			  <xsl:value-of select="@installmentdate" />
			</td>
		  <td class="colW1">{lbl:AccountRemark}</td>
		  <td class="colW2">
			  <xsl:value-of select="@accountremark" />
		  </td>
	  </tr>
      <tr>        
        <td class="colW1">{lbl:IsGroupHeadline}</td>
        <td class="colW2">
          <xsl:value-of select="@isgroupheadline" />
        </td>
        <td class="colW1">{lbl:Checker}</td>
        <td class="colW2">
          <xsl:value-of select="@checker" />
        </td>
	    <td class="colW1">{lbl:IP}</td>
	    <td class="colW2">
		  <xsl:value-of select="@ip" />
	    </td>
      </tr>
      <tr>        
        <td class="colW1">{lbl:ErrCode}</td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1">{lbl:ErrMsg}</td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
		<td class="colW1"></td>
		<td class="colW2"></td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
