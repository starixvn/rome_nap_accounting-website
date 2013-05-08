<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerupdselectedbetting">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          {lbl:Custid}
        </td>
        <td class="colW2">
          <xsl:value-of select="@custid" />
        </td>
        <td class="colW1">
          {lbl:SelectedBetting}
        </td>
        <td class="colW2">
          <xsl:value-of select="@selectedbetting" />
        </td>
        <td class="colW1">
          {lbl:RemoveDownlines}
        </td>
        <td class="colW2">
          <xsl:value-of select="@removedownlines" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          {lbl:userID}
        </td>
        <td class="colW2">
          <xsl:value-of select="@userid" />
        </td>
        <td class="colW1">
          {lbl:ErrCode}
        </td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>         
        <td class="colW1">
          {lbl:ErrMsg}
        </td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          {lbl:IP}
        </td>
        <td class="colW2">
          <xsl:value-of select="@ip" />
        </td>
        <td class="colW1"></td>
        <td class="colW2"></td>
        <td class="colW1"></td>
        <td class="colW2"></td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
